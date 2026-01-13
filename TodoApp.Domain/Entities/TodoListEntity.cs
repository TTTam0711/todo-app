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
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Domain thường không cần RowVersion/Audit đầy đủ ở MVP
        // nhưng bạn có thể giữ RowVersion để xử lý concurrency
        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("List name is required");
            Name = name.Trim();
        }

        public void Archive() => IsArchived = true;
        public void Unarchive() => IsArchived = false;
    }
}
