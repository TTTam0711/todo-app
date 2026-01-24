using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Domain.Entities.Enums
{
    public enum TodoTaskStatus
    {
        Todo = 0,
        InProgress = 1,
        Done = 2,
        Blocked = 3,
        Cancelled = 4
    }
}
