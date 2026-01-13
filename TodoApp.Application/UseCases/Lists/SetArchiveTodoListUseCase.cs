using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces.Repositories;

namespace TodoApp.Application.UseCases.Lists
{
    public class SetArchiveTodoListUseCase
    {
        private readonly ITodoListRepository _repo;

        public SetArchiveTodoListUseCase(ITodoListRepository repo)
        {
            _repo = repo;
        }

        public async Task ExecuteAsync(Guid listId, bool isArchived, CancellationToken ct = default)
        {
            var list = await _repo.GetByIdAsync(listId, ct)
                ?? throw new KeyNotFoundException("TodoList not found");

            if (isArchived)
                list.Archive();
            else
                list.Unarchive();

            await _repo.UpdateAsync(list, ct);
        }
    }

}
