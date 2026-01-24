using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Contracts.TodoTasks.Enums;

namespace TodoApp.Contracts.TodoTasks
{
    public record CreateTodoTaskRequest(
        Guid ListId,
        string Title,
        string? Description,
        TodoTaskPriority Priority,
       DateTimeOffset? DueAt
    );
}
