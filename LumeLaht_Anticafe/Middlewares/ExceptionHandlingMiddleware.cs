using System.Net;
using System.Text.Json;
using LumeLaht_RoomApi.Application.Dto;

namespace LumeLaht_Anticafe.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
                InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };

            _logger.LogError(exception, "Exception: {Message}", exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponseDto
            {
                StatusCode = (int)statusCode,
                Message = message
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}