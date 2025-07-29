using System.Text.Json;
using UserDirectory.Interfaces;
using UserDirectory.Middleware;
using UserDirectory.Repository;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

// Optional, for clarity
// app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
