using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Domain.Entities
{
    public class SubtaskEntity
    {
        public Guid SubtaskId { get; private set; }
        public Guid TaskId { get; private set; }

        public string Title { get; private set; } = string.Empty;
        public bool IsDone { get; private set; }
        public decimal OrderIndex { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

        public void Rename(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Subtask title is required");
            Title = title.Trim();
        }

        public void MarkDone() => IsDone = true;
        public void MarkUndone() => IsDone = false;
    }
}
