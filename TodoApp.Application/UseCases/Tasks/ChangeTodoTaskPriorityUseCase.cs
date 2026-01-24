using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Application.Mappings;
using TodoApp.Domain.Entities.Enums;

namespace TodoApp.Application.UseCases.Tasks
{
    public class ChangeTodoTaskPriorityUseCase
    {
        private readonly ITodoTaskRepository _repo;

        public ChangeTodoTaskPriorityUseCase(ITodoTaskRepository repo)
        {
            _repo = repo;
        }

        public async Task ExecuteAsync(
            Guid taskId,
            Contracts.TodoTasks.Enums.TodoTaskPriority priority,
            CancellationToken ct = default)
        {
            var task = await _repo.GetByIdAsync(taskId, ct)
                ?? throw new KeyNotFoundException("TodoTask not found");

            var domainPriority =
                TodoTaskPriorityMapper.ToDomain(priority);

            task.ChangePriority(domainPriority);

            await _repo.UpdateAsync(task, ct);
        }
    }
}
