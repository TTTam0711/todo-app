using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Domain.Entities
{
    public class TodoListEntity
    {
        public Guid ListId { get; private set; }
        public Guid OwnerUserId { get; private set; }

        public string Name { get; private set; } = string.Empty;
        public string? Color { get; private set; }

        public bool IsArchived { get; private set; }

        // Soft delete
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // Audit
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Concurrency
        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

        // ===== Domain behaviors =====

        public void Rename(string name)
        {
            name = (name ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("List name is required");
            if (name.Length > 100)
                throw new ArgumentException("List name max length is 100");

            Name = name;
            Touch();
        }

        public void ChangeColor(string? color)
        {
            // Optional: validate format "#RRGGBB" nếu bạn muốn
            Color = string.IsNullOrWhiteSpace(color) ? null : color.Trim();
            Touch();
        }

        public void Archive()
        {
            IsArchived = true;
            Touch();
        }

        public void Unarchive()
        {
            IsArchived = false;
            Touch();
        }

        public void SoftDelete(DateTime deletedAt)
        {
            if (IsDeleted) return;

            IsDeleted = true;
            DeletedAt = deletedAt;
            Touch();
        }

        // ===== Internal setters for Factory/Mapping =====
        internal void SetIdentity(Guid listId, Guid ownerUserId)
        {
            ListId = listId;
            OwnerUserId = ownerUserId;
        }

        internal void SetCreated(DateTime createdAt)
        {
            CreatedAt = createdAt;
            UpdatedAt = createdAt;
        }

        internal void SetUpdated(DateTime updatedAt)
        {
            UpdatedAt = updatedAt;
        }

        internal void SetArchived(bool isArchived) => IsArchived = isArchived;
        internal void SetDeleted(bool isDeleted, DateTime? deletedAt)
        {
            IsDeleted = isDeleted;
            DeletedAt = deletedAt;
        }

        internal void SetRowVersion(byte[] rowVersion) => RowVersion = rowVersion ?? Array.Empty<byte>();

        private void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
