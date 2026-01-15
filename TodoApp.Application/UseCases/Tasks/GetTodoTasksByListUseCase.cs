using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Contracts.TodoTasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.UseCases.Tasks
{
    public class GetTodoTasksByListUseCase
    {
        private readonly ITodoTaskRepository _repo;

        public GetTodoTasksByListUseCase(ITodoTaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<TodoTaskDto>> ExecuteAsync(
            Guid listId,
            CancellationToken ct = default)
        {
            var tasks = await _repo.GetByListIdAsync(listId, ct);

            return tasks.Select(t => new TodoTaskDto(
                t.TaskId,
                t.ListId,
                t.Title,
                t.Status.ToString(),
                t.Priority,
                t.OrderIndex,
                t.CreatedAt,
                t.UpdatedAt
            )).ToList();
        }
    }

}
