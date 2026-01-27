using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Contracts.TodoTasks
{
    public record ChangeTodoTaskDescriptionRequest(
    string? Description
);
}
