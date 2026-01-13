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
using TodoApp.Infrastructure.Persistence.Models;

namespace TodoApp.Infrastructure.Repositories
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly TodoAppDbContext _db;
        private readonly IMapper _mapper;

        public TodoListRepository(TodoAppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<TodoListEntity?> GetByIdAsync(Guid listId, CancellationToken ct = default)
        {
            var model = await _db.TodoLists.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ListId == listId && !x.IsDeleted, ct);

            return model is null ? null : _mapper.Map<TodoListEntity>(model);
        }

        public async Task<IReadOnlyList<TodoListEntity>> GetByOwnerAsync(Guid ownerUserId, CancellationToken ct = default)
        {
            var models = await _db.TodoLists.AsNoTracking()
                .Where(x => x.OwnerUserId == ownerUserId && !x.IsDeleted)
                .OrderByDescending(x => x.UpdatedAt)
                .ToListAsync(ct);

            return _mapper.Map<List<TodoListEntity>>(models);
        }

        public async Task AddAsync(TodoListEntity entity, CancellationToken ct = default)
        {
            var model = _mapper.Map<TodoList>(entity);

            _db.TodoLists.Add(model);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(TodoListEntity entity, CancellationToken ct = default)
        {
            var model = await _db.TodoLists
                .FirstOrDefaultAsync(x => x.ListId == entity.ListId, ct)
                ?? throw new KeyNotFoundException("TodoList not found");

            _mapper.Map(entity, model);
            await _db.SaveChangesAsync(ct);
        }

        public async Task SoftDeleteAsync(Guid listId, DateTime deletedAt, CancellationToken ct = default)
        {
            var model = await _db.TodoLists
                .FirstOrDefaultAsync(x => x.ListId == listId, ct)
                ?? throw new KeyNotFoundException("TodoList not found");

            model.IsDeleted = true;
            model.DeletedAt = deletedAt;

            await _db.SaveChangesAsync(ct);
        }

        public async Task SetArchivedAsync(Guid listId, bool isArchived, CancellationToken ct = default)
        {
            var model = await _db.TodoLists
                .FirstOrDefaultAsync(x => x.ListId == listId && !x.IsDeleted, ct)
                ?? throw new KeyNotFoundException("TodoList not found");

            model.IsArchived = isArchived;
            model.UpdatedAt = DateTime.UtcNow; // optional

            await _db.SaveChangesAsync(ct);
        }
    }
}
