using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Contracts.TodoTasks
{
    public record ChangeTodoTaskDueDateRequest
    {
        public DateTimeOffset? DueAt { get; init; }

        public ChangeTodoTaskDueDateRequest(DateTimeOffset? dueAt)
        {
            DueAt = dueAt;
        }
    }
}
