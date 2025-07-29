using System.Net;
using System.Text.Json;
using UserDirectory.Models;

namespace UserDirectory.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // continue to next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message); // log the exception
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new ErrorResponse
                {
                    Status = 500,
                    Message = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred.",
                    StackTrace = _env.IsDevelopment() ? ex.StackTrace : null
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
