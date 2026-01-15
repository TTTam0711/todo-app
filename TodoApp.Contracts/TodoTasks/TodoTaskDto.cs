using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Contracts.TodoTasks
{
    public record TodoTaskDto(
        Guid TaskId,
        Guid ListId,
        string Title,
        string Status,
        byte Priority,
        decimal OrderIndex,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
