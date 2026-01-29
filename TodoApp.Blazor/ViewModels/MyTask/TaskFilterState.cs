using TodoApp.Contracts.TodoTasks.Enums;

namespace TodoApp.Blazor.ViewModels.MyTask
{
    /// <summary>
    /// UI state for task filtering & sorting.
    /// Used ONLY on client side.
    /// </summary>
    public sealed class TaskFilterState
    {
        // =====================
        // Filters
        // =====================

        public TodoTaskStatus? Status { get; set; }
        public TodoTaskPriority? Priority { get; set; }

        // =====================
        // Sort
        // =====================

        public bool SortByDue { get; set; }
        public SortDirection DueDirection { get; set; } = SortDirection.Asc;

        // =====================
        // Helpers
        // =====================

        public void ClearStatus()
        {
            Status = null;
        }

        public void ClearPriority()
        {
            Priority = null;
        }

        public void ClearAll()
        {
            Status = null;
            Priority = null;
            SortByDue = false;
            DueDirection = SortDirection.Asc;
        }

        // =====================
        // Clone (for UI snapshot)
        // =====================

        public TaskFilterState Clone()
        {
            return new TaskFilterState
            {
                Status = Status,
                Priority = Priority,
                SortByDue = SortByDue,
                DueDirection = DueDirection
            };
        }

        // =====================
        // Equality (dirty check)
        // =====================

        public override bool Equals(object? obj)
        {
            if (obj is not TaskFilterState other)
                return false;

            return
                Status == other.Status &&
                Priority == other.Priority &&
                SortByDue == other.SortByDue &&
                DueDirection == other.DueDirection;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                Status,
                Priority,
                SortByDue,
                DueDirection);
        }
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }
}
