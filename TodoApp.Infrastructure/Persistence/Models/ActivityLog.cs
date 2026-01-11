using System;
using System.Collections.Generic;

namespace TodoApp.Infrastructure.Persistence.Models;

public partial class ActivityLog
{
    public long ActivityId { get; set; }

    public Guid ActorUserId { get; set; }

    public string ResourceType { get; set; } = null!;

    public Guid ResourceId { get; set; }

    public string Action { get; set; } = null!;

    public string? DetailJson { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AppUser ActorUser { get; set; } = null!;
}
