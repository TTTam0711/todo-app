using System;
using System.Collections.Generic;

namespace TodoApp.Infrastructure.Persistence.Models;

public partial class Comment
{
    public Guid CommentId { get; set; }

    public Guid TaskId { get; set; }

    public Guid UserId { get; set; }

    public string Body { get; set; } = null!;

    public bool IsEdited { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual TodoTask Task { get; set; } = null!;

    public virtual AppUser User { get; set; } = null!;
}
