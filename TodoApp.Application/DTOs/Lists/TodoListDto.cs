using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Application.DTOs.Lists
{
    public record TodoListDto(
    Guid ListId,
    Guid OwnerUserId,
    string Name,
    string? Color,
    bool IsArchived,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
}
