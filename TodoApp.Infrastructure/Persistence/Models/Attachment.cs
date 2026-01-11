using System;
using System.Collections.Generic;

namespace TodoApp.Infrastructure.Persistence.Models;

public partial class Attachment
{
    public Guid AttachmentId { get; set; }

    public Guid TaskId { get; set; }

    public string FileName { get; set; } = null!;

    public string? ContentType { get; set; }

    public long? SizeBytes { get; set; }

    public string StorageUrl { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual TodoTask Task { get; set; } = null!;
}
