using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Contracts.TodoLists;
using TodoApp.Application.Interfaces.Repositories;
using TodoApp.Domain.Factories;

namespace TodoApp.Application.UseCases.Lists
{
    public class CreateTodoListUseCase
    {
        private readonly ITodoListRepository _repo;

        public CreateTodoListUseCase(ITodoListRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> ExecuteAsync(Guid ownerUserId, CreateTodoListRequest request, CancellationToken ct = default)
        {
            var entity = TodoListFactory.CreateNew(ownerUserId, request.Name, request.Color);
            await _repo.AddAsync(entity, ct);
            return entity.ListId;
        }
    }
}
