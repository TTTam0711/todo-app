using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TodoApp.Application.Mappings
{
    public static class TodoTaskPriorityMapper
    {
        // Domain → Contract (để trả DTO)
        public static Contracts.TodoTasks.Enums.TodoTaskPriority ToContract(
            Domain.Entities.Enums.TodoTaskPriority priority)
            => priority switch
            {
                Domain.Entities.Enums.TodoTaskPriority.None
                    => Contracts.TodoTasks.Enums.TodoTaskPriority.None,

                Domain.Entities.Enums.TodoTaskPriority.High
                    => Contracts.TodoTasks.Enums.TodoTaskPriority.High,

                Domain.Entities.Enums.TodoTaskPriority.Medium
                    => Contracts.TodoTasks.Enums.TodoTaskPriority.Medium,

                Domain.Entities.Enums.TodoTaskPriority.Low
                    => Contracts.TodoTasks.Enums.TodoTaskPriority.Low,

                Domain.Entities.Enums.TodoTaskPriority.Optional
                    => Contracts.TodoTasks.Enums.TodoTaskPriority.Optional,

                _ => throw new ArgumentOutOfRangeException(nameof(priority))
            };

        // Contract → Domain (để nhận request)
        public static Domain.Entities.Enums.TodoTaskPriority ToDomain(
            Contracts.TodoTasks.Enums.TodoTaskPriority priority)
            => priority switch
            {
                Contracts.TodoTasks.Enums.TodoTaskPriority.None
                    => Domain.Entities.Enums.TodoTaskPriority.None,

                Contracts.TodoTasks.Enums.TodoTaskPriority.High
                    => Domain.Entities.Enums.TodoTaskPriority.High,

                Contracts.TodoTasks.Enums.TodoTaskPriority.Medium
                    => Domain.Entities.Enums.TodoTaskPriority.Medium,

                Contracts.TodoTasks.Enums.TodoTaskPriority.Low
                    => Domain.Entities.Enums.TodoTaskPriority.Low,

                Contracts.TodoTasks.Enums.TodoTaskPriority.Optional
                    => Domain.Entities.Enums.TodoTaskPriority.Optional,

                _ => throw new ArgumentOutOfRangeException(nameof(priority))
            };
    }
}
