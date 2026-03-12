using System.Net.Http.Json;
using TodoApp.Contracts.Auth;

namespace TodoApp.Blazor.Services
{
    public sealed class AuthApiClient
    {
        private readonly HttpClient _http;

        public AuthApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task RegisterAsync(RegisterRequest request, CancellationToken ct = default)
        {
            var res = await _http.PostAsJsonAsync("api/auth/register", request, ct);

            if (!res.IsSuccessStatusCode)
            {
                var msg = await res.Content.ReadAsStringAsync(ct);
                throw new InvalidOperationException(msg);
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            var res = await _http.PostAsJsonAsync("api/auth/login", request, ct);

            if (!res.IsSuccessStatusCode)
            {
                var msg = await res.Content.ReadAsStringAsync(ct);
                throw new InvalidOperationException(msg);
            }

            return await res.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: ct)
                   ?? throw new InvalidOperationException("Invalid login response");
        }
    }
}
