using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Entities.Enums;

namespace TodoApp.Application.Queries.Tasks
{
    public sealed class QueryTodoTasksInternal
    {
        public Guid ListId { get; init; }

        // Domain enums
        public TodoTaskStatus? Status { get; init; }
        public TodoTaskPriority? Priority { get; init; }

        public bool IncludeCompleted { get; init; }
        public bool IncludeDeleted { get; init; }

        public DateTimeOffset? DueFrom { get; init; }
        public DateTimeOffset? DueTo { get; init; }

        public TodoTaskSortBy SortBy { get; init; }
        public SortDirection Direction { get; init; }
    }
}
