using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Contracts.TodoTasks.Enums;

namespace TodoApp.Contracts.TodoTasks
{
    public record TodoTaskDetailDto(
        Guid TaskId,
        Guid ListId,

        string Title,
        string? Description,

        TodoTaskStatus Status,
        TodoTaskPriority Priority,

        DateTimeOffset? DueAt,
        DateTimeOffset? CompletedAt,

        DateTime CreatedAt,
        DateTime UpdatedAt,

        IReadOnlyList<TodoTaskStatus> AllowedStatusTransitions
    );
}
