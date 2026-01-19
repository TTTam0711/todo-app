using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Contracts.TodoTasks;
using TodoApp.Domain.Factories;

namespace TodoApp.Application.UseCases.Tasks
{
    public class CreateTodoTaskUseCase
    {
        private readonly ITodoTaskRepository _taskRepo;
        private readonly ITodoListRepository _listRepo;

        public CreateTodoTaskUseCase(
            ITodoTaskRepository taskRepo,
            ITodoListRepository listRepo)
        {
            _taskRepo = taskRepo;
            _listRepo = listRepo;
        }

        public async Task<Guid> ExecuteAsync(
            CreateTodoTaskRequest request,
            Guid userId,
            CancellationToken ct = default)
        {
            var list = await _listRepo.GetByIdAsync(request.ListId, ct)
                ?? throw new KeyNotFoundException("TodoList not found");

            if (list.IsArchived)
                throw new InvalidOperationException("Cannot add task to archived list");

            var lastOrder = await _taskRepo.GetMaxOrderIndexAsync(request.ListId, ct);

            var task = TodoTaskFactory.CreateNew(
                request.ListId,
                request.Title,
                userId,
                lastOrder + 1,
                request.DueAt,
                request.Description,
                request.Priority
            );

            await _taskRepo.AddAsync(task, ct);

            return task.TaskId;
        }
    }
}
