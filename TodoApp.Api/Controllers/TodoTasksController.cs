using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Mappings;
using TodoApp.Application.Queries.Tasks;
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
        private readonly GetTodoTaskDetailUseCase _getDetail;
        private readonly RenameTodoTaskUseCase _rename;
        private readonly ChangeTodoTaskStatusUseCase _changeStatus;
        private readonly ReorderTodoTaskUseCase _reorder;
        private readonly DeleteTodoTaskUseCase _delete;
        private readonly ChangeTodoTaskDueDateUseCase _changeDueDate;
        private readonly ChangeTodoTaskPriorityUseCase _changePriority;
        private readonly ChangeTodoTaskDescriptionUseCase _changeDescription;
        private readonly QueryTodoTasksUseCase _queryTasks;
        private readonly ILogger<TodoTasksController> _logger;

        public TodoTasksController(
            CreateTodoTaskUseCase create,
            GetTodoTasksByListUseCase getByList,
            GetTodoTaskDetailUseCase getDetail,
            RenameTodoTaskUseCase rename,
            ChangeTodoTaskStatusUseCase changeStatus,
            ChangeTodoTaskDueDateUseCase changeDueDate,
            ChangeTodoTaskPriorityUseCase changePriority,
            ReorderTodoTaskUseCase reorder,
            DeleteTodoTaskUseCase delete,
            ChangeTodoTaskDescriptionUseCase changeDescription,
            QueryTodoTasksUseCase queryTasks,
            ILogger<TodoTasksController> logger)
        {
            _create = create;
            _getByList = getByList;
            _getDetail = getDetail;
            _rename = rename;
            _changeStatus = changeStatus;
            _changeDueDate = changeDueDate;
            _changePriority = changePriority;
            _reorder = reorder;
            _delete = delete;
            _changeDescription = changeDescription;
            _queryTasks = queryTasks;
            _logger = logger;
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
        // GET /api/todotasks/{taskId}
        // ================================
        [HttpGet("todotasks/{taskId:guid}")]
        public async Task<ActionResult<TodoTaskDetailDto>> GetDetail(
            Guid taskId,
            CancellationToken ct)
        {
            try
            {
                var result = await _getDetail.ExecuteAsync(taskId, ct);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
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

        // PATCH /api/todotasks/{taskId}/description
        [HttpPatch("todotasks/{taskId:guid}/description")]
        public async Task<IActionResult> ChangeDescription(
            Guid taskId,
            [FromBody] ChangeTodoTaskDescriptionRequest request,
            CancellationToken ct)
        {
            await _changeDescription.ExecuteAsync(
                taskId,
                request.Description,
                ct);

            return NoContent();
        }

        [HttpGet("todolists/{listId:guid}/tasks/query")]
        public async Task<ActionResult<IReadOnlyList<TodoTaskListItemDto>>> Query(
            Guid listId,
            [FromQuery] GetTodoTasksQueryRequest request,
            CancellationToken ct)
        {
            // ================================
            // Parse sorting parameters
            // ================================
            var parsedSortBy =
                Enum.TryParse<TodoTaskSortBy>(
                    request.SortBy,
                    ignoreCase: true,
                    out var sortBy)
                    ? sortBy
                    : TodoTaskSortBy.OrderIndex;

            var parsedDirection =
                Enum.TryParse<SortDirection>(
                    request.Direction,
                    ignoreCase: true,
                    out var direction)
                    ? direction
                    : SortDirection.Asc;
            _logger.LogInformation(
            "QueryTodoTasks: RawSortBy={RawSortBy}, ParsedSortBy={ParsedSortBy}, RawDirection={RawDirection}, ParsedDirection={ParsedDirection}",
            request.SortBy,
            parsedSortBy,
            request.Direction,
            parsedDirection);
            var query = new QueryTodoTasks
            {
                ListId = listId,
                Status = request.Status,
                Priority = request.Priority,
                IncludeCompleted = request.IncludeCompleted,
                IncludeDeleted = request.IncludeDeleted,
                DueFrom = request.DueFrom,
                DueTo = request.DueTo,
                SortBy = parsedSortBy,
                Direction = parsedDirection
            };

            var result = await _queryTasks.ExecuteAsync(query, ct);
            return Ok(result);
        }
    }
}
