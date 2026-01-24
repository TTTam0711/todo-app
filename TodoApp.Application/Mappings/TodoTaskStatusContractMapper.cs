using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainStatus = TodoApp.Domain.Entities.Enums.TodoTaskStatus;
using ContractStatus = TodoApp.Contracts.TodoTasks.Enums.TodoTaskStatus;

namespace TodoApp.Application.Mappings
{
    internal static class TodoTaskStatusContractMapper
    {
        public static DomainStatus ToDomain(ContractStatus status)
            => status switch
            {
                ContractStatus.Todo => DomainStatus.Todo,
                ContractStatus.InProgress => DomainStatus.InProgress,
                ContractStatus.Done => DomainStatus.Done,
                ContractStatus.Blocked => DomainStatus.Blocked,
                ContractStatus.Cancelled => DomainStatus.Cancelled,
                _ => throw new ArgumentOutOfRangeException(nameof(status))
            };

        public static ContractStatus ToContract(DomainStatus status)
            => status switch
            {
                DomainStatus.Todo => ContractStatus.Todo,
                DomainStatus.InProgress => ContractStatus.InProgress,
                DomainStatus.Done => ContractStatus.Done,
                DomainStatus.Blocked => ContractStatus.Blocked,
                DomainStatus.Cancelled => ContractStatus.Cancelled,
                _ => throw new ArgumentOutOfRangeException(nameof(status))
            };
    }
}
