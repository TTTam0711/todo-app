using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Contracts.TodoTasks.Enums;

namespace TodoApp.Contracts.TodoTasks
{
    public record ChangeTodoTaskPriorityRequest
    {
        public TodoTaskPriority Priority { get; init; }

        public ChangeTodoTaskPriorityRequest(TodoTaskPriority priority)
        {
            Priority = priority;
        }
    }
}
