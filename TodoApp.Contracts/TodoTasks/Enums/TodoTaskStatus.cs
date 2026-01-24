using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Contracts.TodoTasks.Enums
{
    public enum TodoTaskStatus : byte
    {
        Todo = 0,
        InProgress = 1,
        Done = 2,
        Blocked = 3,
        Cancelled = 4
    }
}
