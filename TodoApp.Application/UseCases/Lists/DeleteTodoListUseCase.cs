using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;

namespace TodoApp.Application.UseCases.Lists
{
    public class DeleteTodoListUseCase
    {
        private readonly ITodoListRepository _repo;

        public DeleteTodoListUseCase(ITodoListRepository repo)
        {
            _repo = repo;
        }

        public async Task ExecuteAsync(Guid listId, CancellationToken ct = default)
        {
            var list = await _repo.GetByIdAsync(listId, ct)
                ?? throw new KeyNotFoundException("TodoList not found");

            list.SoftDelete(DateTime.UtcNow);

            await _repo.UpdateAsync(list, ct);
        }
    }

}
