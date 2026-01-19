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

        public decimal OrderIndex { get; private set; }

        // Soft delete = state
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // Audit
        public Guid CreatedByUserId { get; private set; }
        public Guid UpdatedByUserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTimeOffset? DueAt { get; private set; }
        public DateTimeOffset? CompletedAt { get; private set; }
        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

        // ===== Domain behavior =====

        public void Rename(string title)
        {
            title = (title ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Task title is required");

            Title = title;
            Touch();
        }

        public void ChangeStatus(TodoTaskStatus newStatus)
        {
            if (Status == newStatus)
                return;

            ValidateStatusTransition(Status, newStatus);

            Status = newStatus;

            if (newStatus == TodoTaskStatus.Done)
            {
                CompletedAt = DateTimeOffset.UtcNow;
            }

            Touch();
        }

        private static void ValidateStatusTransition(
            TodoTaskStatus current,
            TodoTaskStatus next)
        {
            var allowed = current switch
            {
                TodoTaskStatus.Todo =>
                    next is TodoTaskStatus.InProgress
                         or TodoTaskStatus.Cancelled,

                TodoTaskStatus.InProgress =>
                    next is TodoTaskStatus.Done
                         or TodoTaskStatus.Blocked
                         or TodoTaskStatus.Cancelled,

                TodoTaskStatus.Blocked =>
                    next is TodoTaskStatus.InProgress
                         or TodoTaskStatus.Cancelled,

                TodoTaskStatus.Done =>
                    false,        // terminal

                TodoTaskStatus.Cancelled =>
                    false,        // terminal

                _ => false
            };

            if (!allowed)
                throw new InvalidOperationException(
                    $"Cannot change status from {current} to {next}");
        }

        public void SoftDelete(DateTime deletedAt)
        {
            if (IsDeleted) return;

            IsDeleted = true;
            DeletedAt = deletedAt;
            Touch();
        }
        public void UpdateDescription(string? description)
        {
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();

            Touch();
        }

        public void ChangePriority(byte priority)
        {
            if (priority > 5)
                throw new ArgumentOutOfRangeException(
                    nameof(priority),
                    "Priority must be between 0 and 5");

            Priority = priority;
            Touch();
        }

        // ===== Internal setters for Factory =====

        internal void SetIdentity(Guid taskId, Guid listId)
        {
            TaskId = taskId;
            ListId = listId;
        }

        internal void SetCreated(DateTime createdAt, Guid userId)
        {
            CreatedAt = createdAt;
            UpdatedAt = createdAt;
            CreatedByUserId = userId;
            UpdatedByUserId = userId;
        }

        internal void SetUpdated(DateTime updatedAt, Guid userId)
        {
            UpdatedAt = updatedAt;
            UpdatedByUserId = userId;
        }

        internal void SetDeleted(bool isDeleted, DateTime? deletedAt)
        {
            IsDeleted = isDeleted;
            DeletedAt = deletedAt;
        }

        internal void SetOrder(decimal orderIndex) => OrderIndex = orderIndex;
        internal void SetPriority(byte priority) => Priority = priority;
        internal void SetRowVersion(byte[] rowVersion) => RowVersion = rowVersion ?? Array.Empty<byte>();
        internal void SetDueAt(DateTimeOffset? dueAt)
        {
            DueAt = dueAt;
        }
        internal void SetCompletedAt(DateTimeOffset? completedAt)
        {
            CompletedAt = completedAt;
        }
        internal void SetDescription(string? description)
        {
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();
        }
        private void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }
        public void Reorder(decimal newOrderIndex)
        {
            if (newOrderIndex < 0)
                throw new ArgumentException("OrderIndex must be >= 0");

            OrderIndex = newOrderIndex;
            Touch();
        }

        internal void SetStatus(TodoTaskStatus status)
        {
            Status = status;
        }
    }

}
