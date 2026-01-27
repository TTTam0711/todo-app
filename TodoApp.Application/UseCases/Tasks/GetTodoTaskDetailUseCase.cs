using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Application.Mappings;
using TodoApp.Contracts.TodoTasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.UseCases.Tasks
{
    public class GetTodoTaskDetailUseCase
    {
        private readonly ITodoTaskRepository _repo;

        public GetTodoTaskDetailUseCase(ITodoTaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<TodoTaskDetailDto> ExecuteAsync(
            Guid taskId,
            CancellationToken ct = default)
        {
            var task = await _repo.GetByIdAsync(taskId, ct)
                ?? throw new KeyNotFoundException("TodoTask not found");

            return MapToDetailDto(task);
        }

        // ===============================
        // Read-model projection
        // ===============================

        private static TodoTaskDetailDto MapToDetailDto(TodoTaskEntity task)
        {
            var allowedStatuses =
                task.GetAllowedStatusTransitions()
                    .Select(TodoTaskStatusContractMapper.ToContract)
                    .ToList();

            return new TodoTaskDetailDto(
                task.TaskId,
                task.ListId,
                task.Title,
                task.Description,
                TodoTaskStatusContractMapper.ToContract(task.Status),
                TodoTaskPriorityMapper.ToContract(task.Priority),
                task.DueAt,
                task.CompletedAt,
                task.CreatedAt,
                task.UpdatedAt,
                allowedStatuses
            );
        }
    }
}
