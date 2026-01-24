using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;

namespace TodoApp.Application.UseCases.Tasks
{
    public class ChangeTodoTaskDueDateUseCase
    {
        private readonly ITodoTaskRepository _repo;

        public ChangeTodoTaskDueDateUseCase(ITodoTaskRepository repo)
        {
            _repo = repo;
        }

        public async Task ExecuteAsync(
            Guid taskId,
            DateTimeOffset? dueAt,
            CancellationToken ct = default)
        {
            var task = await _repo.GetByIdAsync(taskId, ct)
                ?? throw new KeyNotFoundException("TodoTask not found");

            task.ChangeDueDate(dueAt);

            await _repo.UpdateAsync(task, ct);
        }
    }
}
