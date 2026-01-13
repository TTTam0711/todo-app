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
    public class TagRepository : ITagRepository
    {
        private readonly TodoAppDbContext _db;
        private readonly IMapper _mapper;

        public TagRepository(TodoAppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<TagEntity>> GetByOwnerAsync(Guid ownerUserId, CancellationToken ct = default)
        {
            var models = await _db.Tags.AsNoTracking()
                .Where(x => x.OwnerUserId == ownerUserId)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);

            return _mapper.Map<List<TagEntity>>(models);
        }

        public async Task<TagEntity?> GetByIdAsync(Guid tagId, CancellationToken ct = default)
        {
            var model = await _db.Tags.AsNoTracking()
                .FirstOrDefaultAsync(x => x.TagId == tagId, ct);

            return model is null ? null : _mapper.Map<TagEntity>(model);
        }

        public async Task AddAsync(TagEntity entity, CancellationToken ct = default)
        {
            var model = _mapper.Map<Tag>(entity);

            _db.Tags.Add(model);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(TagEntity entity, CancellationToken ct = default)
        {
            var model = await _db.Tags
                .FirstOrDefaultAsync(x => x.TagId == entity.TagId, ct)
                ?? throw new KeyNotFoundException("Tag not found");

            _mapper.Map(entity, model);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid tagId, CancellationToken ct = default)
        {
            var model = await _db.Tags
                .FirstOrDefaultAsync(x => x.TagId == tagId, ct)
                ?? throw new KeyNotFoundException("Tag not found");

            _db.Tags.Remove(model);
            await _db.SaveChangesAsync(ct);
        }
    }

}
