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
        private readonly ChangeTodoTaskDueDateUseCase _changeDueDate;
        private readonly ChangeTodoTaskPriorityUseCase _changePriority;
        public TodoTasksController(
            CreateTodoTaskUseCase create,
            GetTodoTasksByListUseCase getByList,
            RenameTodoTaskUseCase rename,
            ChangeTodoTaskStatusUseCase changeStatus,
            ChangeTodoTaskDueDateUseCase changeDueDate,
            ChangeTodoTaskPriorityUseCase changePriority,
            ReorderTodoTaskUseCase reorder,
            DeleteTodoTaskUseCase delete)
        {
            _create = create;
            _getByList = getByList;
            _rename = rename;
            _changeStatus = changeStatus;
            _changeDueDate = changeDueDate;
            _changePriority = changePriority;
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
            await _changeStatus.ExecuteAsync(
                taskId,
                request.Status,   // <-- Contract enum
                ct);

            return NoContent();
        }

        // PATCH: api/todotasks/{taskId}/duedate
        [HttpPatch("todotasks/{taskId:guid}/due")]
        public async Task<IActionResult> ChangeDueDate(
            Guid taskId,
            [FromBody] ChangeTodoTaskDueDateRequest request,
            CancellationToken ct)
        {
            await _changeDueDate.ExecuteAsync(
                taskId,
                request.DueAt,
                ct);

            return NoContent();
        }

        // PATCH: api/todotasks/{taskId}/priority
        [HttpPatch("todotasks/{taskId:guid}/priority")]
        public async Task<IActionResult> ChangePriority(
            Guid taskId,
            [FromBody] ChangeTodoTaskPriorityRequest request,
            CancellationToken ct)
        {
            try
            {
                await _changePriority.ExecuteAsync(
                 taskId,
                 request.Priority, 
                 ct
             );
                return NoContent();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    allowedRange = "0–4"
                });
            }
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
        public async Task<IActionResult> Delete(Guid taskId, CancellationToken ct)
        {
            await _delete.ExecuteAsync(taskId, ct);
            return NoContent();
        }
    }
}
