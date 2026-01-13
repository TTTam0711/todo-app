using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.DTOs.Lists;
using TodoApp.Application.Interfaces.Repositories;

namespace TodoApp.Application.UseCases.Lists
{
    public class GetTodoListsUseCase
    {
        private readonly ITodoListRepository _repo;

        public GetTodoListsUseCase(ITodoListRepository repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<TodoListDto>> ExecuteAsync(Guid ownerUserId, CancellationToken ct = default)
        {
            var lists = await _repo.GetByOwnerAsync(ownerUserId, ct);

            return lists.Select(x => new TodoListDto(
                x.ListId, x.OwnerUserId, x.Name, x.Color, x.IsArchived, x.CreatedAt, x.UpdatedAt
            )).ToList();
        }
    }
}
