using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Contracts.TodoLists;
using TodoApp.Application.UseCases.Lists;

namespace ToDoApp.Api.Controllers
{
    [ApiController]
    [Route("api/todolists")]
    public class TodoListsController : ControllerBase
    {
        private readonly CreateTodoListUseCase _create;
        private readonly UpdateTodoListUseCase _update;
        private readonly DeleteTodoListUseCase _delete;
        private readonly SetArchiveTodoListUseCase _archive;
        private readonly GetTodoListByIdUseCase _getById;
        private readonly GetTodoListsUseCase _getByOwner;

        public TodoListsController(
            CreateTodoListUseCase create,
            UpdateTodoListUseCase update,
            DeleteTodoListUseCase delete,
            SetArchiveTodoListUseCase archive,
            GetTodoListByIdUseCase getById,
            GetTodoListsUseCase getByOwner)
        {
            _create = create;
            _update = update;
            _delete = delete;
            _archive = archive;
            _getById = getById;
            _getByOwner = getByOwner;
        }

        // POST /api/todolists
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateTodoListRequest request,
            CancellationToken ct)
        {
            // tạm hard-code ownerUserId (sau này lấy từ JWT)
            var ownerUserId = Guid.Parse(User.FindFirst("sub")?.Value
                ?? throw new UnauthorizedAccessException());

            var id = await _create.ExecuteAsync(ownerUserId, request, ct);

            return CreatedAtAction(nameof(GetById), new { listId = id }, null);
        }

        // GET /api/todolists/{listId}
        [HttpGet("{listId:guid}")]
        public async Task<ActionResult<TodoListDto>> GetById(
            Guid listId,
            CancellationToken ct)
        {
            var result = await _getById.ExecuteAsync(listId, ct);
            return Ok(result);
        }

        // GET /api/todolists?ownerUserId=...
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TodoListDto>>> GetByOwner(
            [FromQuery] Guid ownerUserId,
            CancellationToken ct)
        {
            var result = await _getByOwner.ExecuteAsync(ownerUserId, ct);
            return Ok(result);
        }

        // PUT /api/todolists/{listId}
        [HttpPut("{listId:guid}")]
        public async Task<IActionResult> Update(
            Guid listId,
            [FromBody] UpdateTodoListRequest request,
            CancellationToken ct)
        {
            await _update.ExecuteAsync(listId, request, ct);
            return NoContent();
        }

        // PATCH /api/todolists/{listId}/archive
        [HttpPatch("{listId:guid}/archive")]
        public async Task<IActionResult> SetArchive(
            Guid listId,
            [FromQuery] bool isArchived,
            CancellationToken ct)
        {
            await _archive.ExecuteAsync(listId, isArchived, ct);
            return NoContent();
        }

        // DELETE /api/todolists/{listId}
        [HttpDelete("{listId:guid}")]
        public async Task<IActionResult> Delete(
            Guid listId,
            CancellationToken ct)
        {
            await _delete.ExecuteAsync(listId, ct);
            return NoContent();
        }
    }

}
