using System;
using System.Collections.Generic;

namespace TodoApp.Infrastructure.Persistence.Models;

public partial class Tag
{
    public Guid TagId { get; set; }

    public Guid OwnerUserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Color { get; set; }

    public DateTime CreatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual AppUser OwnerUser { get; set; } = null!;

    public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();
}
