using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<IReadOnlyList<TagEntity>> GetByOwnerAsync(Guid ownerUserId, CancellationToken ct = default);
        Task<TagEntity?> GetByIdAsync(Guid tagId, CancellationToken ct = default);

        Task AddAsync(TagEntity entity, CancellationToken ct = default);
        Task UpdateAsync(TagEntity entity, CancellationToken ct = default);
        Task DeleteAsync(Guid tagId, CancellationToken ct = default);
    }
}
