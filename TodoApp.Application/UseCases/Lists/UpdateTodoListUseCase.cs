using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.DTOs.Lists;
using TodoApp.Application.Interfaces.Repositories;

namespace TodoApp.Application.UseCases.Lists
{
    public class UpdateTodoListUseCase
    {
        private readonly ITodoListRepository _repo;

        public UpdateTodoListUseCase(ITodoListRepository repo)
        {
            _repo = repo;
        }

        public async Task ExecuteAsync(Guid listId, UpdateTodoListRequest request, CancellationToken ct = default)
        {
            var list = await _repo.GetByIdAsync(listId, ct)
                ?? throw new KeyNotFoundException("TodoList not found");

            // Domain rule inside entity
            list.Rename(request.Name);
            list.ChangeColor(request.Color);

            await _repo.UpdateAsync(list, ct);
        }
    }
}
