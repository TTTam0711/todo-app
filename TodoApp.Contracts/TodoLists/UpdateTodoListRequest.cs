using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Contracts.TodoLists
{
    public record UpdateTodoListRequest(string Name, string? Color);
}
