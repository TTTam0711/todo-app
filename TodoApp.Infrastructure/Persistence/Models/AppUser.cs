using System;
using System.Collections.Generic;

namespace TodoApp.Infrastructure.Persistence.Models;

public partial class AppUser
{
    public Guid UserId { get; set; }

    public string Email { get; set; } = null!;

    public byte[]? PasswordHash { get; set; }

    public string DisplayName { get; set; } = null!;

    public string? TimeZone { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<Share> Shares { get; set; } = new List<Share>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<TodoList> TodoLists { get; set; } = new List<TodoList>();

    public virtual ICollection<TodoTask> TodoTaskCreatedByUsers { get; set; } = new List<TodoTask>();

    public virtual ICollection<TodoTask> TodoTaskUpdatedByUsers { get; set; } = new List<TodoTask>();
}
