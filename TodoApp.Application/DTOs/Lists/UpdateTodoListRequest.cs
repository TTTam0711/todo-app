using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Application.DTOs.Lists
{
    public record UpdateTodoListRequest(string Name, string? Color);
}
