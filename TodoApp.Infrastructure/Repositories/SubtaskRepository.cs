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
    public class SubtaskRepository : ISubtaskRepository
    {
        private readonly TodoAppDbContext _db;
        private readonly IMapper _mapper;

        public SubtaskRepository(TodoAppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<SubtaskEntity>> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default)
        {
            var models = await _db.Subtasks.AsNoTracking()
                .Where(x => x.TaskId == taskId)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync(ct);

            return _mapper.Map<List<SubtaskEntity>>(models);
        }

        public async Task<SubtaskEntity?> GetByIdAsync(Guid subtaskId, CancellationToken ct = default)
        {
            var model = await _db.Subtasks.AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubtaskId == subtaskId, ct);

            return model is null ? null : _mapper.Map<SubtaskEntity>(model);
        }

        public async Task AddAsync(SubtaskEntity entity, CancellationToken ct = default)
        {
            var model = _mapper.Map<Subtask>(entity);

            _db.Subtasks.Add(model);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(SubtaskEntity entity, CancellationToken ct = default)
        {
            var model = await _db.Subtasks
                .FirstOrDefaultAsync(x => x.SubtaskId == entity.SubtaskId, ct)
                ?? throw new KeyNotFoundException("Subtask not found");

            _mapper.Map(entity, model);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid subtaskId, CancellationToken ct = default)
        {
            var model = await _db.Subtasks
                .FirstOrDefaultAsync(x => x.SubtaskId == subtaskId, ct)
                ?? throw new KeyNotFoundException("Subtask not found");

            _db.Subtasks.Remove(model);
            await _db.SaveChangesAsync(ct);
        }
    }

}
