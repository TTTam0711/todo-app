using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Domain.Entities
{
    public class TodoTaskEntity
    {
        public Guid TaskId { get; private set; }
        public Guid ListId { get; private set; }

        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public TodoTaskStatus Status { get; private set; }
        public byte Priority { get; private set; }

        public DateTimeOffset? StartAt { get; private set; }
        public DateTimeOffset? DueAt { get; private set; }
        public DateTimeOffset? CompletedAt { get; private set; }

        public int? EstimatedMinutes { get; private set; }

        // Recurring
        public bool IsRecurring { get; private set; }
        public string? RecurrenceRule { get; private set; }

        // Sorting in list
        public decimal OrderIndex { get; private set; }

        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public Guid CreatedByUserId { get; private set; }
        public Guid UpdatedByUserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

        public void Rename(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required");
            Title = title.Trim();
        }

        public void MarkDone(DateTimeOffset completedAt)
        {
            Status = TodoTaskStatus.Done;
            CompletedAt = completedAt;
        }

        public void ChangeStatus(TodoTaskStatus status)
        {
            Status = status;
            if (status != TodoTaskStatus.Done) CompletedAt = null;
        }

        public void SoftDelete(DateTime deletedAt)
        {
            IsDeleted = true;
            DeletedAt = deletedAt;
        }
    }
}
