using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Contracts.TodoTasks.Enums
{
    public enum TodoTaskPriority : byte
    {
        None = 0,
        High = 1,
        Medium = 2,
        Low = 3,
        Optional = 4
    }
}
