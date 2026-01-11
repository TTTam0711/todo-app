using System;
using System.Collections.Generic;

namespace TodoApp.Infrastructure.Persistence.Models;

public partial class TodoList
{
    public Guid ListId { get; set; }

    public Guid OwnerUserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Color { get; set; }

    public bool IsArchived { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual AppUser OwnerUser { get; set; } = null!;

    public virtual ICollection<TodoTask> TodoTasks { get; set; } = new List<TodoTask>();
}
