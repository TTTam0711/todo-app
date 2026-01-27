using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Queries.Tasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces.Repositories
{
    public interface ITodoTaskRepository
    {
        Task<TodoTaskEntity?> GetByIdAsync(
            Guid taskId,
            CancellationToken ct = default);

        Task<IReadOnlyList<TodoTaskEntity>> GetByListIdAsync(
            Guid listId,
            CancellationToken ct = default);

        Task<decimal> GetMaxOrderIndexAsync(
            Guid listId,
            CancellationToken ct = default);

        Task AddAsync(
            TodoTaskEntity entity,
            CancellationToken ct = default);

        Task UpdateAsync(
            TodoTaskEntity entity,
            CancellationToken ct = default);
        Task<IReadOnlyList<TodoTaskEntity>> QueryAsync(
            QueryTodoTasksInternal query,
            CancellationToken ct);
    }

}
