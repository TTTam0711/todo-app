using System;
using System.Collections.Generic;

namespace TodoApp.Infrastructure.Persistence.Models;

public partial class AiParsedInput
{
    public Guid ParsedId { get; set; }

    public Guid UserId { get; set; }

    public string RawText { get; set; } = null!;

    public string ParsedJson { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
