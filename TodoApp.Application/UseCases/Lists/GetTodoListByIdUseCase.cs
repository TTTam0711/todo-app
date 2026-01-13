using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.DTOs.Lists;
using TodoApp.Application.Interfaces.Repositories;

namespace TodoApp.Application.UseCases.Lists
{
    public class GetTodoListByIdUseCase
    {
        private readonly ITodoListRepository _repo;

        public GetTodoListByIdUseCase(ITodoListRepository repo)
        {
            _repo = repo;
        }

        public async Task<TodoListDto> ExecuteAsync(Guid listId, CancellationToken ct = default)
        {
            var x = await _repo.GetByIdAsync(listId, ct)
                ?? throw new KeyNotFoundException("TodoList not found");

            return new TodoListDto(x.ListId, x.OwnerUserId, x.Name, x.Color, x.IsArchived, x.CreatedAt, x.UpdatedAt);
        }
    }
}
