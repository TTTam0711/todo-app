using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Contracts.TodoTasks.Enums;

namespace TodoApp.Contracts.TodoTasks
{
    public class GetTodoTasksQueryRequest
    {
        // ===== Filters =====
        public TodoTaskStatus? Status { get; set; }
        public TodoTaskPriority? Priority { get; set; }

        public bool IncludeCompleted { get; set; } = true;
        public bool IncludeDeleted { get; set; } = false;

        public DateTimeOffset? DueFrom { get; set; }
        public DateTimeOffset? DueTo { get; set; }

        // ===== Sorting =====
        public string? SortBy { get; set; }
        public string? Direction { get; set; }
    }
}
