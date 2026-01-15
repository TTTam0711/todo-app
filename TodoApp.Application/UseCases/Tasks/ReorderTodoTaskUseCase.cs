using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Contracts.TodoTasks;

namespace TodoApp.Application.UseCases.Tasks
{
    public class ReorderTodoTaskUseCase
    {
        private readonly ITodoTaskRepository _repo;

        public ReorderTodoTaskUseCase(ITodoTaskRepository repo)
        {
            _repo = repo;
        }

        public async Task ExecuteAsync(
            Guid taskId,
            ReorderTodoTaskRequest request,
            CancellationToken ct = default)
        {
            var task = await _repo.GetByIdAsync(taskId, ct)
                ?? throw new KeyNotFoundException("TodoTask not found");

            task.Reorder(request.OrderIndex);

            await _repo.UpdateAsync(task, ct);
        }
    }

}
