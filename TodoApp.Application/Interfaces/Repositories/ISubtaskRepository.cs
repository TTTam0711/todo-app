using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces.Repositories
{
    public interface ISubtaskRepository
    {
        Task<IReadOnlyList<SubtaskEntity>> GetByTaskIdAsync(Guid taskId, CancellationToken ct = default);
        Task<SubtaskEntity?> GetByIdAsync(Guid subtaskId, CancellationToken ct = default);

        Task AddAsync(SubtaskEntity entity, CancellationToken ct = default);
        Task UpdateAsync(SubtaskEntity entity, CancellationToken ct = default);
        Task DeleteAsync(Guid subtaskId, CancellationToken ct = default);
    }
}
