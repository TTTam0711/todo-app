using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Factories
{
    public static class TodoListFactory
    {
        public static TodoListEntity CreateNew(
            Guid ownerUserId,
            string name,
            string? color)
        {
            if (ownerUserId == Guid.Empty)
                throw new ArgumentException("OwnerUserId is required");

            name = (name ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("List name is required");
            if (name.Length > 100)
                throw new ArgumentException("List name max length is 100");

            var now = DateTime.UtcNow;

            var entity = new TodoListEntity();
            entity.SetIdentity(Guid.NewGuid(), ownerUserId);
            entity.SetCreated(now);

            entity.Rename(name);       // reuse rule
            entity.ChangeColor(color); // normalize

            entity.Unarchive();
            entity.SetDeleted(false, null);

            return entity;
        }

        public static TodoListEntity RestoreFromDb(
            Guid listId,
            Guid ownerUserId,
            string name,
            string? color,
            bool isArchived,
            bool isDeleted,
            DateTime? deletedAt,
            DateTime createdAt,
            DateTime updatedAt,
            byte[] rowVersion)
        {
            var entity = new TodoListEntity();

            entity.SetIdentity(listId, ownerUserId);
            entity.SetCreated(createdAt);
            entity.SetUpdated(updatedAt);
            entity.SetArchived(isArchived);
            entity.SetDeleted(isDeleted, deletedAt);
            entity.SetRowVersion(rowVersion);

            entity.Rename(name);
            entity.ChangeColor(color);

            return entity;
        }
    }

}
