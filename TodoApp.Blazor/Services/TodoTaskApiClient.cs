using System.Net.Http.Json;
using TodoApp.Blazor.ViewModels.MyTask;
using TodoApp.Contracts.TodoTasks;
using TodoApp.Contracts.TodoTasks.Enums;

namespace TodoApp.Blazor.Services
{
    public class TodoTaskApiClient
    {
        private readonly HttpClient _http;

        public TodoTaskApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<IReadOnlyList<TodoTaskListItemDto>> GetByListIdAsync(
            Guid listId,
            CancellationToken ct = default)
        {
            var url = $"api/todolists/{listId}/tasks";

            return await _http.GetFromJsonAsync<List<TodoTaskListItemDto>>(url, ct)
                   ?? new List<TodoTaskListItemDto>();
        }
        public async Task CreateAsync(Guid listId, CreateTodoTaskRequest request)
        {
            var response = await _http.PostAsJsonAsync(
                $"api/todolists/{listId}/tasks",
                request
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task ChangeStatusAsync(
            Guid taskId,
            ChangeTodoTaskStatusRequest request)
        {
            var response = await _http.PatchAsJsonAsync(
                $"api/todotasks/{taskId}/status",
                request
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task RenameAsync(
               Guid taskId,
               RenameTodoTaskRequest request)
        {
            var response = await _http.PatchAsJsonAsync(
                $"api/todotasks/{taskId}/rename",
                request
            );

            response.EnsureSuccessStatusCode();
        }
        public async Task ChangeDueDateAsync(
            Guid taskId,
            ChangeTodoTaskDueDateRequest request)
        {
            await _http.PatchAsJsonAsync(
                $"api/todotasks/{taskId}/due",
                request);
        }
        public async Task ChangePriorityAsync(
            Guid taskId,
            ChangeTodoTaskPriorityRequest request,
            CancellationToken ct = default)
        {
            var response = await _http.PatchAsJsonAsync(
                $"api/todotasks/{taskId}/priority",
                request,
                ct);

            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteAsync(Guid taskId, CancellationToken ct = default)
        {
            var response = await _http.DeleteAsync(
                $"api/todotasks/{taskId}",
                ct);

            response.EnsureSuccessStatusCode();
        }
        public async Task<TodoTaskDetailDto> GetDetailAsync(Guid taskId)
        {
            return await _http.GetFromJsonAsync<TodoTaskDetailDto>(
                $"api/todotasks/{taskId}")
                ?? throw new InvalidOperationException("Task not found");
        }
        public async Task ChangeDescriptionAsync(
            Guid taskId,
            ChangeTodoTaskDescriptionRequest request,
            CancellationToken ct = default)
        {
            var response = await _http.PatchAsJsonAsync(
                $"api/todotasks/{taskId}/description",
                request,
                ct);

            response.EnsureSuccessStatusCode();
        }
        public async Task<IReadOnlyList<TodoTaskListItemDto>> QueryAsync(
            Guid listId,
            TaskFilterState filter,
            CancellationToken ct)
        {
            var query = new Dictionary<string, string>();

            if (filter.Status.HasValue)
                query["status"] = ((int)filter.Status.Value).ToString();

            if (filter.Priority.HasValue)
                query["priority"] = ((int)filter.Priority.Value).ToString();

            if (filter.SortByDue)
            {
                query["sortBy"] = "DueAt";
                query["direction"] =
                    filter.DueDirection == SortDirection.Desc ? "desc" : "asc";
            }

            var url =
                $"api/todolists/{listId}/tasks/query?" +
                string.Join("&", query.Select(x => $"{x.Key}={x.Value}"));

            return await _http.GetFromJsonAsync<List<TodoTaskListItemDto>>(url, ct)
                ?? new List<TodoTaskListItemDto>();
        }

        //QueryAsync dùng cho calendar  
        public async Task<IReadOnlyList<TodoTaskListItemDto>> QueryAsync(
            Guid listId,
            TaskFilterState filter,
            DateTimeOffset? dueFrom,
            DateTimeOffset? dueTo,
            CancellationToken ct = default)
        {
            var query = new Dictionary<string, string>();
            query["includeCompleted"] = "true";
            if (filter.Status.HasValue)
                query["status"] = ((int)filter.Status.Value).ToString();

            if (filter.Priority.HasValue)
                query["priority"] = ((int)filter.Priority.Value).ToString();

            if (filter.SortByDue)
            {
                query["sortBy"] = "DueAt";
                query["direction"] =
                    filter.DueDirection == SortDirection.Desc ? "desc" : "asc";
            }

            // 🔑 Calendar date range
            if (dueFrom.HasValue)
                query["dueFrom"] =
                    Uri.EscapeDataString(dueFrom.Value.ToString("O"));

            if (dueTo.HasValue)
                query["dueTo"] =
                    Uri.EscapeDataString(dueTo.Value.ToString("O"));

            var url =
                $"api/todolists/{listId}/tasks/query?" +
                string.Join("&", query.Select(x => $"{x.Key}={x.Value}"));

            return await _http.GetFromJsonAsync<List<TodoTaskListItemDto>>(url, ct)
                ?? new List<TodoTaskListItemDto>();
        }

    }
}
