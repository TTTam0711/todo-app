using System;
using System.Collections.Generic;

namespace TodoApp.Infrastructure.Persistence.Models;

public partial class Reminder
{
    public Guid ReminderId { get; set; }

    public Guid TaskId { get; set; }

    public DateTimeOffset RemindAt { get; set; }

    public DateTimeOffset? SentAt { get; set; }

    public string Channel { get; set; } = null!;

    public virtual TodoTask Task { get; set; } = null!;
}
