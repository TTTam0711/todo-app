using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace TodoApp.Blazor.ViewModels.Auth
{
    public class CookieHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // 🔥 Bắt buộc để browser gửi cookie refresh_token
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
