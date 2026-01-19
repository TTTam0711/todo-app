using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.UseCases.Tasks;
using TodoApp.Contracts.TodoTasks;
using TodoApp.Domain.Entities;

namespace ToDoApp.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class TodoTasksController : ControllerBase
    {
        // ===== UseCases =====
        private readonly CreateTodoTaskUseCase _create;
        private readonly GetTodoTasksByListUseCase _getByList;
        private readonly RenameTodoTaskUseCase _rename;
        private readonly ChangeTodoTaskStatusUseCase _changeStatus;
        private readonly ReorderTodoTaskUseCase _reorder;
        private readonly DeleteTodoTaskUseCase _delete;

        public TodoTasksController(
            CreateTodoTaskUseCase create,
            GetTodoTasksByListUseCase getByList,
            RenameTodoTaskUseCase rename,
            ChangeTodoTaskStatusUseCase changeStatus,
            ReorderTodoTaskUseCase reorder,
            DeleteTodoTaskUseCase delete)
        {
            _create = create;
            _getByList = getByList;
            _rename = rename;
            _changeStatus = changeStatus;
            _reorder = reorder;
            _delete = delete;
        }

        // ================================
        // GET /api/todolists/{listId}/tasks
        // ================================
        [HttpGet("todolists/{listId:guid}/tasks")]
        public async Task<ActionResult<IReadOnlyList<TodoTaskListItemDto>>> GetByList(
                        Guid listId,
                        CancellationToken ct)
        {
            var result = await _getByList.ExecuteAsync(listId, ct);
            return Ok(result);
        }

        // ================================
        // POST /api/todolists/{listId}/tasks
        // ================================
        [HttpPost("todolists/{listId:guid}/tasks")]
        public async Task<IActionResult> Create(
                Guid listId,
                [FromBody] CreateTodoTaskRequest request,
                CancellationToken ct)
        {
            var userId = Guid.Parse("7D97D295-C6F0-F011-9685-782B464EB9F6");

            // đảm bảo listId khớp route
            request = request with { ListId = listId };

            var taskId = await _create.ExecuteAsync(request, userId, ct);

            return CreatedAtAction(
                nameof(GetByList),
                new { listId },
                new { taskId }
            );
        }


        // ================================
        // PATCH /api/todotasks/{taskId}/rename
        // ================================
        [HttpPatch("todotasks/{taskId:guid}/rename")]
        public async Task<IActionResult> Rename(
            Guid taskId,
            [FromBody] RenameTodoTaskRequest request,
            CancellationToken ct)
        {
            await _rename.ExecuteAsync(taskId, request, ct);
            return NoContent();
        }


        // ================================
        // PATCH /api/todotasks/{taskId}/status
        // ================================
        [HttpPatch("todotasks/{taskId:guid}/status")]
        public async Task<IActionResult> ChangeStatus(
            Guid taskId,
            [FromBody] ChangeTodoTaskStatusRequest request,
            CancellationToken ct)
        {
            if (!Enum.TryParse<TodoTaskStatus>(
                    request.Status,
                    ignoreCase: true,
                    out var status))
            {
                return BadRequest("Invalid task status");
            }

            await _changeStatus.ExecuteAsync(taskId, status, ct);
            return NoContent();
        }

        // ================================
        // PATCH /api/todotasks/{taskId}/reorder
        // ================================
        [HttpPatch("todotasks/{taskId:guid}/reorder")]
        public async Task<IActionResult> Reorder(
            Guid taskId,
            [FromBody] ReorderTodoTaskRequest request,
            CancellationToken ct)
        {
            await _reorder.ExecuteAsync(taskId, request, ct);
            return NoContent();
        }


        // ================================
        // DELETE /api/todotasks/{taskId}
        // ================================
        [HttpDelete("todotasks/{taskId:guid}")]
        public async Task<IActionResult> Delete(
            Guid taskId,
            CancellationToken ct)
        {
            await _delete.ExecuteAsync(taskId, ct);
            return NoContent();
        }
    }
}
