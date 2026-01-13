using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Application.DTOs.Tasks
{
    public record CreateTodoTaskRequest(
    Guid ListId,
    string Title,
    string? Description,
    byte Priority,
    DateTimeOffset? DueAt
);
}
