using System;
using System.Collections.Generic;

namespace TodoApp.Infrastructure.Persistence.Models;

public partial class Share
{
    public Guid ShareId { get; set; }

    public string ResourceType { get; set; } = null!;

    public Guid ResourceId { get; set; }

    public Guid UserId { get; set; }

    public string Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual AppUser User { get; set; } = null!;
}
