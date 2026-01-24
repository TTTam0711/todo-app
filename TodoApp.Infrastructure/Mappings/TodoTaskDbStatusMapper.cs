using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Entities.Enums;

namespace TodoApp.Infrastructure.Mappings
{
    internal static class TodoTaskDbStatusMapper
    {
        public static TodoTaskStatus ToDomain(string status)
            => status switch
            {
                "Todo" => TodoTaskStatus.Todo,
                "InProgress" => TodoTaskStatus.InProgress,
                "Done" => TodoTaskStatus.Done,
                "Blocked" => TodoTaskStatus.Blocked,
                "Cancelled" => TodoTaskStatus.Cancelled,
                _ => throw new InvalidOperationException(
                    $"Invalid TodoTaskStatus from DB: '{status}'")
            };

        public static string ToDb(TodoTaskStatus status)
            => status switch
            {
                TodoTaskStatus.Todo => "Todo",
                TodoTaskStatus.InProgress => "InProgress",
                TodoTaskStatus.Done => "Done",
                TodoTaskStatus.Blocked => "Blocked",
                TodoTaskStatus.Cancelled => "Cancelled",
                _ => throw new ArgumentOutOfRangeException(nameof(status))
            };
    }
}
