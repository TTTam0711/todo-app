using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Entities.Enums;

namespace TodoApp.Domain.Factories
{
    public static class TodoTaskFactory
    {
        public static TodoTaskEntity CreateNew(
            Guid listId,
            string title,
            Guid createdByUserId,
            decimal orderIndex,
            DateTimeOffset? dueAt,
            string? description,
            TodoTaskPriority priority = TodoTaskPriority.None)
        {
            if (listId == Guid.Empty)
                throw new ArgumentException("ListId is required");

            if (createdByUserId == Guid.Empty)
                throw new ArgumentException("CreatedByUserId is required");

            title = (title ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Task title is required");

            var now = DateTime.UtcNow;

            var entity = new TodoTaskEntity();
            entity.SetIdentity(Guid.NewGuid(), listId);
            entity.SetCreated(now, createdByUserId);

            entity.Rename(title);
            entity.SetDescription(null);
            entity.UpdateDescription(description);
            entity.ChangePriority(priority);
            entity.ChangeStatus(TodoTaskStatus.Todo);
            entity.SetOrder(orderIndex);
            entity.SetDueAt(dueAt);
            entity.SetCompletedAt(null);
            entity.SetDeleted(false, null);

            return entity;
        }

        public static TodoTaskEntity RestoreFromDb(
            Guid taskId,
            Guid listId,
            string title,
            TodoTaskStatus status,
            bool isDeleted,
            DateTime? deletedAt,
            decimal orderIndex,
           TodoTaskPriority priority,
            string? description,
            Guid createdByUserId,
            Guid updatedByUserId,
            DateTime createdAt,
            DateTime updatedAt,
            DateTimeOffset? dueAt,
            DateTimeOffset? completedAt,
            byte[] rowVersion)
        {
            var entity = new TodoTaskEntity();

            entity.SetIdentity(taskId, listId);
            entity.SetCreated(createdAt, createdByUserId);
            entity.SetUpdated(updatedAt, updatedByUserId);
            entity.SetDeleted(isDeleted, deletedAt);
            entity.SetOrder(orderIndex);
            entity.SetPriority(priority);
            entity.SetRowVersion(rowVersion);

            entity.Rename(title);
            entity.SetDescription(description);
            entity.SetStatus(status);
            entity.SetDueAt(dueAt);
            entity.SetCompletedAt(completedAt);
            return entity;
        }
    }

}
