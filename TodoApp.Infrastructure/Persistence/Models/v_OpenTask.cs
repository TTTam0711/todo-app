using System;
using System.Collections.Generic;

namespace TodoApp.Infrastructure.Persistence.Models;

public partial class v_OpenTask
{
    public Guid TaskId { get; set; }

    public string Title { get; set; } = null!;

    public string Status { get; set; } = null!;

    public byte Priority { get; set; }

    public DateTimeOffset? StartAt { get; set; }

    public DateTimeOffset? DueAt { get; set; }

    public decimal OrderIndex { get; set; }

    public Guid ListId { get; set; }

    public string ListName { get; set; } = null!;

    public Guid OwnerUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
