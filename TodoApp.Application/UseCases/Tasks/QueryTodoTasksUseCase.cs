using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Application.Mappings;
using TodoApp.Application.Queries.Tasks;
using TodoApp.Contracts.TodoTasks;
using TodoApp.Domain.Entities.Enums;

namespace TodoApp.Application.UseCases.Tasks
{
    public sealed class QueryTodoTasksUseCase
    {
        private readonly ITodoTaskRepository _repository;

        public QueryTodoTasksUseCase(ITodoTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<TodoTaskListItemDto>> ExecuteAsync(
            QueryTodoTasks query,
            CancellationToken ct)
        {
            // =========================
            // 1️⃣ Map Contract → Domain
            // =========================

            TodoTaskStatus? domainStatus = query.Status.HasValue
                ? TodoTaskStatusContractMapper.ToDomain(query.Status.Value)
                : null;

            TodoTaskPriority? domainPriority = query.Priority.HasValue
                ? TodoTaskPriorityMapper.ToDomain(query.Priority.Value)
                : null;

            // =========================
            // 2️⃣ Build Domain query
            // =========================

            var domainQuery = new QueryTodoTasksInternal
            {
                ListId = query.ListId,
                Status = domainStatus,
                Priority = domainPriority,
                IncludeCompleted = query.IncludeCompleted,
                IncludeDeleted = query.IncludeDeleted,
                DueFrom = query.DueFrom,
                DueTo = query.DueTo,
                SortBy = query.SortBy,
                Direction = query.Direction
            };

            // =========================
            // 3️⃣ Query repository
            // =========================

            var tasks = await _repository.QueryAsync(domainQuery, ct);

            // =========================
            // 4️⃣ Map Domain → DTO
            // =========================

            return tasks.Select(t => new TodoTaskListItemDto(
                TaskId: t.TaskId,
                ListId: t.ListId,
                Title: t.Title,
                Status: TodoTaskStatusContractMapper.ToContract(t.Status),
                Description: t.Description,
                Priority: TodoTaskPriorityMapper.ToContract(t.Priority),
                OrderIndex: t.OrderIndex,
                DueAt: t.DueAt,
                CompletedAt: t.CompletedAt,
                IsOverdue:
                    t.DueAt.HasValue
                    && t.DueAt.Value < DateTimeOffset.UtcNow
                    && t.Status != TodoTaskStatus.Done,
                AllowedStatusTransitions:
                    t.GetAllowedStatusTransitions()
                     .Select(TodoTaskStatusContractMapper.ToContract)
                     .ToList()
            )).ToList();
        }
    }
}
