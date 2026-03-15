using System;
using System.Collections.Generic;

namespace TodoApp.Infrastructure.Persistence.Models;

public partial class Subtask
{
    public Guid SubtaskId { get; set; }

    public Guid TaskId { get; set; }

    public string Title { get; set; } = null!;

    public bool IsDone { get; set; }

    public decimal OrderIndex { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public int? EstimatedMinutes { get; set; }

    public virtual TodoTask Task { get; set; } = null!;
}
