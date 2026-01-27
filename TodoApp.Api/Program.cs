using TodoApp.Application;
using TodoApp.Application.Mappings;
using TodoApp.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var blazorOrigin = "https://localhost:7048"; // ??i theo port Blazor c?a b?n

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorDev", policy =>
        policy.WithOrigins(blazorOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("BlazorDev");

app.UseAuthorization();

app.MapControllers();

app.Run();
