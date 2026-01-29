using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using TodoApp.Blazor;
using TodoApp.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ?? ??c ApiBaseUrl t? wwwroot/appsettings.json
var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException("ApiBaseUrl is not configured");

builder.Services.AddScoped(_ =>
    new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

builder.Services.AddScoped<TodoListApiClient>();
builder.Services.AddScoped<TodoTaskApiClient>();
builder.Services.AddRadzenComponents();
await builder.Build().RunAsync();
