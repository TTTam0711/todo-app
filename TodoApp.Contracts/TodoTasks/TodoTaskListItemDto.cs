using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Contracts.TodoTasks
{
    public record TodoTaskListItemDto(
    Guid TaskId,
    Guid ListId,

    string Title,
    string Status,
    string? Description,
    byte Priority,
    decimal OrderIndex,

    DateTimeOffset? DueAt,
    DateTimeOffset? CompletedAt,

    bool IsOverdue,
    IReadOnlyList<string> AllowedStatusTransitions
);

}
