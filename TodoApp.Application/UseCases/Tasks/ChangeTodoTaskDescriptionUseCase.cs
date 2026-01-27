using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;

namespace TodoApp.Application.UseCases.Tasks
{
    public class ChangeTodoTaskDescriptionUseCase
    {
        private readonly ITodoTaskRepository _repo;

        public ChangeTodoTaskDescriptionUseCase(
            ITodoTaskRepository repo)
        {
            _repo = repo;
        }

        public async Task ExecuteAsync(
            Guid taskId,
            string? description,
            CancellationToken ct = default)
        {
            var task = await _repo.GetByIdAsync(taskId, ct)
                ?? throw new KeyNotFoundException("TodoTask not found");

            task.UpdateDescription(description);

            await _repo.UpdateAsync(task, ct);
        }
    }
}
