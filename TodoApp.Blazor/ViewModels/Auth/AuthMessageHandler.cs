using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TodoApp.Blazor.Services;
using TodoApp.Contracts.Auth;

namespace TodoApp.Blazor.ViewModels.Auth
{
    public sealed class AuthMessageHandler : DelegatingHandler
    {
        private readonly AuthState _authState;
        private readonly HttpClient _http;

        public AuthMessageHandler(AuthState authState, HttpClient http)
        {
            _authState = authState;
            _http = http;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.AbsolutePath?.ToLowerInvariant() ?? "";

            // 🔥 Không attach token / không refresh cho các endpoint auth
            var isAuthEndpoint =
                path.StartsWith("/api/auth/login") ||
                path.StartsWith("/api/auth/register") ||
                path.StartsWith("/api/auth/refresh") ||
                path.StartsWith("/api/auth/logout");

            // 0) Chặn retry vô hạn
            if (request.Headers.Contains("X-Retry"))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            // 1) Attach access token nếu có
            if (!isAuthEndpoint && !string.IsNullOrWhiteSpace(_authState.AccessToken))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authState.AccessToken);
            }

            // 2) Gửi request lần đầu
            var response = await base.SendAsync(request, cancellationToken);

            // 3) Nếu không phải 401 => return luôn
            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            // 4) Nếu auth endpoint mà bị 401 => return luôn
            if (isAuthEndpoint)
                return response;

            // 5) Nếu 401 => thử refresh 1 lần
            try
            {
                var res = await _http.PostAsync("api/auth/refresh", null, cancellationToken);

                if (!res.IsSuccessStatusCode)
                {
                    _authState.Clear();
                    return response;
                }

                var refreshed = await res.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: cancellationToken);

                if (refreshed == null)
                {
                    _authState.Clear();
                    return response;
                }
            }
            catch
            {
                // Refresh fail => clear auth
                _authState.Clear();
                return response;
            }

            // 6) Retry request 1 lần với token mới
            var retryRequest = await CloneRequestAsync(request);

            retryRequest.Headers.Add("X-Retry", "1");

            retryRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _authState.AccessToken);

            response.Dispose(); // tránh leak
            return await base.SendAsync(retryRequest, cancellationToken);
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            // Copy headers
            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            // Copy content
            if (request.Content != null)
            {
                var bytes = await request.Content.ReadAsByteArrayAsync();
                clone.Content = new ByteArrayContent(bytes);

                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            // Copy options (NET 6+)
            foreach (var opt in request.Options)
                clone.Options.Set(new HttpRequestOptionsKey<object?>(opt.Key), opt.Value);

            return clone;
        }
    }

}
