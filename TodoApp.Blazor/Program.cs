using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using TodoApp.Blazor;
using TodoApp.Blazor.Services;
using TodoApp.Blazor.ViewModels.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException("ApiBaseUrl is not configured");

// HttpClient c? b?n
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(apiBaseUrl)
    });

// State
builder.Services.AddScoped<AuthState>();

// Api Clients
builder.Services.AddScoped<AuthApiClient>();
builder.Services.AddScoped<TodoListApiClient>();
builder.Services.AddScoped<TodoTaskApiClient>();

builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();
