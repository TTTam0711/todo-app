using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Entities.Enums;

namespace TodoApp.Infrastructure.Mappings
{
    internal static class TodoTaskDbPriorityMapper
    {
        // =========================
        // Domain → DB
        // =========================
        public static byte ToDb(TodoTaskPriority priority)
            => priority switch
            {
                TodoTaskPriority.None => 0,
                TodoTaskPriority.High => 1,
                TodoTaskPriority.Medium => 2,
                TodoTaskPriority.Low => 3,
                TodoTaskPriority.Optional => 4,
                _ => throw new ArgumentOutOfRangeException(nameof(priority))
            };

        // =========================
        // DB → Domain
        // =========================
        public static TodoTaskPriority ToDomain(byte priority)
            => priority switch
            {
                0 => TodoTaskPriority.None,
                1 => TodoTaskPriority.High,
                2 => TodoTaskPriority.Medium,
                3 => TodoTaskPriority.Low,
                4 => TodoTaskPriority.Optional,
                _ => throw new InvalidOperationException(
                    $"Invalid TodoTaskPriority from DB: '{priority}'")
            };
    }
}
