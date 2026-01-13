using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Domain.Entities
{
    public class TagEntity
    {
        public Guid TagId { get; private set; }
        public Guid OwnerUserId { get; private set; }

        public string Name { get; private set; } = string.Empty;
        public string? Color { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tag name is required");
            Name = name.Trim();
        }

        public void ChangeColor(string? color) => Color = color;
    }
}
