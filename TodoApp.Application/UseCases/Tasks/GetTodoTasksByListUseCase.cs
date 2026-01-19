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

        public async Task<IReadOnlyList<TodoTaskListItemDto>> ExecuteAsync(
            Guid listId,
            CancellationToken ct = default)
        {
            var tasks = await _repo.GetByListIdAsync(listId, ct);

            return tasks.Select(t => new TodoTaskListItemDto(
                t.TaskId,
                t.ListId,

                t.Title,
                t.Status.ToString(),
                t.Description,
                t.Priority,
                t.OrderIndex,

                t.DueAt,
                t.CompletedAt,

                IsOverdue(t),
                GetAllowedStatusTransitions(t.Status)
            )).ToList();
        }

        // ===============================
        // Read-side derived state
        // ===============================

        private static bool IsOverdue(TodoTaskEntity task)
        {
            if (task.DueAt == null)
                return false;

            if (task.Status is TodoTaskStatus.Done
                or TodoTaskStatus.Cancelled)
                return false;

            return task.DueAt < DateTimeOffset.UtcNow;
        }

        // ===============================
        // Status transition projection
        // ===============================

        private static IReadOnlyList<string> GetAllowedStatusTransitions(
            TodoTaskStatus currentStatus)
        {
            return currentStatus switch
            {
                TodoTaskStatus.Todo => new[]
                {
                nameof(TodoTaskStatus.InProgress),
                nameof(TodoTaskStatus.Cancelled)
            },

                TodoTaskStatus.InProgress => new[]
                {
                nameof(TodoTaskStatus.Done),
                nameof(TodoTaskStatus.Blocked),
                nameof(TodoTaskStatus.Cancelled)
            },

                TodoTaskStatus.Blocked => new[]
                {
                nameof(TodoTaskStatus.InProgress),
                nameof(TodoTaskStatus.Cancelled)
            },

                TodoTaskStatus.Done => Array.Empty<string>(),
                TodoTaskStatus.Cancelled => Array.Empty<string>(),

                _ => Array.Empty<string>()
            };
        }
    }
}
