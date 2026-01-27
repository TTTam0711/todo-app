using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Application.Mappings;
using TodoApp.Contracts.TodoTasks;
using TodoApp.Domain.Entities;
using DomainStatus = TodoApp.Domain.Entities.Enums.TodoTaskStatus;


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

            return tasks.Select(t =>
            {
                var allowedContractStatuses =
                    t.GetAllowedStatusTransitions()
                     .Select(TodoTaskStatusContractMapper.ToContract)
                     .ToList();

                return new TodoTaskListItemDto(
                    t.TaskId,
                    t.ListId,
                    t.Title,
                    TodoTaskStatusContractMapper.ToContract(t.Status), 
                    t.Description,
                    TodoTaskPriorityMapper.ToContract(t.Priority),
                    t.OrderIndex,
                    t.DueAt,
                    t.CompletedAt,
                    IsOverdue(t),
                    allowedContractStatuses
                );
            }).ToList();
        }

        // ===============================
        // Read-side derived state
        // ===============================

        private static bool IsOverdue(TodoTaskEntity task)
        {
            if (task.DueAt == null)
                return false;

            if (task.Status is DomainStatus.Done
                or DomainStatus.Cancelled)
                return false;

            return task.DueAt < DateTimeOffset.UtcNow;
        }

        
    }
}

