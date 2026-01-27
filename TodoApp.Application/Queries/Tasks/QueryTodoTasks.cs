using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Contracts.TodoTasks.Enums;


namespace TodoApp.Application.Queries.Tasks
{
    public sealed class QueryTodoTasks
    {
        // ===== Required =====
        public Guid ListId { get; init; }

        // ===== Filters =====
        public TodoTaskStatus? Status { get; init; }
        public TodoTaskPriority? Priority { get; init; }

        public bool IncludeCompleted { get; init; } = true;
        public bool IncludeDeleted { get; init; } = false;

        public DateTimeOffset? DueFrom { get; init; }
        public DateTimeOffset? DueTo { get; init; }

        // ===== Sorting =====
        public TodoTaskSortBy SortBy { get; init; } = TodoTaskSortBy.OrderIndex;
        public SortDirection Direction { get; init; } = SortDirection.Asc;
    }
}
