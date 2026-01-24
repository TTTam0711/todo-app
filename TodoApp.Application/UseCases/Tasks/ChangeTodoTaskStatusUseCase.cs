using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Application.Mappings;
using TodoApp.Contracts.TodoTasks.Enums;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.UseCases.Tasks
{
    public class ChangeTodoTaskStatusUseCase
    {
        private readonly ITodoTaskRepository _repo;

        public ChangeTodoTaskStatusUseCase(ITodoTaskRepository repo)
        {
            _repo = repo;
        }

        public async Task ExecuteAsync(
                Guid taskId,
                TodoTaskStatus status,
                CancellationToken ct = default)
        {
            var task = await _repo.GetByIdAsync(taskId, ct)
                ?? throw new KeyNotFoundException("TodoTask not found");

            var domainStatus =
                TodoTaskStatusContractMapper.ToDomain(status);

            task.ChangeStatus(domainStatus);

            await _repo.UpdateAsync(task, ct);
        }

    }

}
