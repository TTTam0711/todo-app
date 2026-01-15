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

        public async Task<IReadOnlyList<TodoTaskDto>> GetByListIdAsync(
            Guid listId,
            CancellationToken ct = default)
        {
            var url = $"api/todolists/{listId}/tasks";

            return await _http.GetFromJsonAsync<List<TodoTaskDto>>(url, ct)
                   ?? new List<TodoTaskDto>();
        }
    }
}
