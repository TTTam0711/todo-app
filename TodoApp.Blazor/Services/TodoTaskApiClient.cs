using System.Net.Http.Json;
using TodoApp.Contracts.TodoTasks;

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
    }
}
