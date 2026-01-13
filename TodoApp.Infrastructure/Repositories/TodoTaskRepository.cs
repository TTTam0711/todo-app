using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Persistence.Context;

namespace TodoApp.Infrastructure.Repositories
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly TodoAppDbContext _db;
        private readonly IMapper _mapper;

        public TodoTaskRepository(TodoAppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<TodoTaskEntity?> GetByIdAsync(Guid taskId, CancellationToken ct = default)
        {
            var model = await _db.TodoTasks.AsNoTracking()
                .FirstOrDefaultAsync(x => x.TaskId == taskId && !x.IsDeleted, ct);

            return model is null ? null : _mapper.Map<TodoTaskEntity>(model);
        }

        public async Task<IReadOnlyList<TodoTaskEntity>> GetByListIdAsync(Guid listId, CancellationToken ct = default)
        {
            var models = await _db.TodoTasks.AsNoTracking()
                .Where(x => x.ListId == listId && !x.IsDeleted)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync(ct);

            return _mapper.Map<List<TodoTaskEntity>>(models);
        }

        public async Task AddAsync(TodoTaskEntity entity, CancellationToken ct = default)
        {
            var model = _mapper.Map<Infrastructure.Persistence.Models.TodoTask>(entity);
            _db.TodoTasks.Add(model);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(TodoTaskEntity entity, CancellationToken ct = default)
        {
            var model = await _db.TodoTasks.FirstOrDefaultAsync(x => x.TaskId == entity.TaskId, ct)
                ?? throw new KeyNotFoundException("TodoTask not found");

            _mapper.Map(entity, model);
            await _db.SaveChangesAsync(ct);
        }

        public async Task SoftDeleteAsync(Guid taskId, DateTime deletedAt, CancellationToken ct = default)
        {
            var model = await _db.TodoTasks.FirstOrDefaultAsync(x => x.TaskId == taskId, ct)
                ?? throw new KeyNotFoundException("TodoTask not found");

            model.IsDeleted = true;
            model.DeletedAt = deletedAt;

            await _db.SaveChangesAsync(ct);
        }
    }
}
