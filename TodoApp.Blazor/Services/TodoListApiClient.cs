using System.Net.Http.Json;
using TodoApp.Contracts.TodoLists;

namespace TodoApp.Blazor.Services
{
    public class TodoListApiClient
    {
        private readonly HttpClient _http;

        public TodoListApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<IReadOnlyList<TodoListDto>> GetByOwnerAsync(Guid ownerUserId)
        {
            var url = $"api/todolists?ownerUserId={ownerUserId}";
            return await _http.GetFromJsonAsync<List<TodoListDto>>(url)
                   ?? new List<TodoListDto>();
        }

        public async Task CreateAsync(CreateTodoListRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/todolists", request);
            response.EnsureSuccessStatusCode();
        }
    }
}
