using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces.Repositories
{
    public interface ITodoListRepository
    {
        Task<TodoListEntity?> GetByIdAsync(Guid listId, CancellationToken ct = default);
        Task<IReadOnlyList<TodoListEntity>> GetByOwnerAsync(Guid ownerUserId, CancellationToken ct = default);

        Task AddAsync(TodoListEntity entity, CancellationToken ct = default);
        Task UpdateAsync(TodoListEntity entity, CancellationToken ct = default);
    }
}
