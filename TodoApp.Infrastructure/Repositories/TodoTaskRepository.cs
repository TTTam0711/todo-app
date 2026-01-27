using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Application.Queries.Tasks;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Mappings;
using TodoApp.Infrastructure.Persistence.Context;
using TodoApp.Infrastructure.Persistence.Models;

namespace TodoApp.Infrastructure.Repositories
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly TodoAppDbContext _db;
        private readonly IMapper _mapper;

        public TodoTaskRepository(
            TodoAppDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<TodoTaskEntity?> GetByIdAsync(
            Guid taskId,
            CancellationToken ct = default)
        {
            var model = await _db.TodoTasks
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.TaskId == taskId && !x.IsDeleted,
                    ct);

            return model == null
                ? null
                : _mapper.Map<TodoTaskEntity>(model);
        }

        public async Task<IReadOnlyList<TodoTaskEntity>> GetByListIdAsync(
            Guid listId,
            CancellationToken ct = default)
        {
            var models = await _db.TodoTasks
                .AsNoTracking()
                .Where(x => x.ListId == listId && !x.IsDeleted)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync(ct);

            return _mapper.Map<List<TodoTaskEntity>>(models);
        }

        public async Task<decimal> GetMaxOrderIndexAsync(
            Guid listId,
            CancellationToken ct = default)
        {
            return await _db.TodoTasks
                .Where(x => x.ListId == listId && !x.IsDeleted)
                .Select(x => (decimal?)x.OrderIndex)
                .MaxAsync(ct)
                ?? 0;
        }

        public async Task AddAsync(
            TodoTaskEntity entity,
            CancellationToken ct = default)
        {
            var model = _mapper.Map<TodoTask>(entity);

            _db.TodoTasks.Add(model);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(
            TodoTaskEntity entity,
            CancellationToken ct = default)
        {
            var model = await _db.TodoTasks
                .FirstOrDefaultAsync(x => x.TaskId == entity.TaskId, ct)
                ?? throw new KeyNotFoundException("TodoTask not found");

            _mapper.Map(entity, model);

            await _db.SaveChangesAsync(ct);
        }
        public async Task<IReadOnlyList<TodoTaskEntity>> QueryAsync(
            QueryTodoTasksInternal q,
            CancellationToken ct)
        {
            IQueryable<TodoTask> query = _db.TodoTasks
                .AsNoTracking()
                .Where(t => t.ListId == q.ListId);

            // =========================
            // Soft delete filter
            // =========================
            if (!q.IncludeDeleted)
            {
                query = query.Where(t => !t.IsDeleted);
            }

            // =========================
            // Status filter
            // =========================
            if (q.Status.HasValue)
            {
                var dbStatus = TodoTaskDbStatusMapper.ToDb(q.Status.Value);
                query = query.Where(t => t.Status == dbStatus);
            }

            // =========================
            // Priority filter
            // =========================
            if (q.Priority.HasValue)
            {
                var dbPriority = TodoTaskDbPriorityMapper.ToDb(q.Priority.Value);
                query = query.Where(t => t.Priority == dbPriority);
            }

            // =========================
            // Completed filter
            // =========================
            if (!q.IncludeCompleted)
            {
                var doneStatus = TodoTaskDbStatusMapper.ToDb(
                    Domain.Entities.Enums.TodoTaskStatus.Done);

                query = query.Where(t => t.Status != doneStatus);
            }

            // =========================
            // Due date filter
            // =========================
            if (q.DueFrom.HasValue)
            {
                query = query.Where(t => t.DueAt >= q.DueFrom.Value);
            }

            if (q.DueTo.HasValue)
            {
                query = query.Where(t => t.DueAt <= q.DueTo.Value);
            }

            // =========================
            // Sorting
            // =========================
            query = ApplySorting(query, q);

            // =========================
            // Execute query
            // =========================
            var rows = await query.ToListAsync(ct);

            // =========================
            // Map EF → Domain
            // =========================
            return rows.Select(_mapper.Map<TodoTaskEntity>).ToList();
        }
        private static IQueryable<TodoTask> ApplySorting(
            IQueryable<TodoTask> query,
            QueryTodoTasksInternal q)
        {
            return (q.SortBy, q.Direction) switch
            {
                (TodoTaskSortBy.DueAt, SortDirection.Asc)
                    => query.OrderBy(t => t.DueAt),

                (TodoTaskSortBy.DueAt, SortDirection.Desc)
                    => query.OrderByDescending(t => t.DueAt),

                (TodoTaskSortBy.Priority, SortDirection.Asc)
                    => query.OrderBy(t => t.Priority),

                (TodoTaskSortBy.Priority, SortDirection.Desc)
                    => query.OrderByDescending(t => t.Priority),

                (TodoTaskSortBy.CreatedAt, SortDirection.Desc)
                    => query.OrderByDescending(t => t.CreatedAt),

                _ => query.OrderBy(t => t.OrderIndex)
            };
        }
    }
}
