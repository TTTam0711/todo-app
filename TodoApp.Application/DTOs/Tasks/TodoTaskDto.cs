using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Application.DTOs.Tasks
{
    public record TodoTaskDto(
    Guid TaskId,
    Guid ListId,
    string Title,
    string? Description,
    string Status,
    byte Priority,
    DateTimeOffset? DueAt,
    DateTimeOffset? CompletedAt,
    decimal OrderIndex
);
}
