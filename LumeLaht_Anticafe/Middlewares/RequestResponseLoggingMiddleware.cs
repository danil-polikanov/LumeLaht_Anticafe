namespace LumeLaht_Anticafe.Middlewares
{
    public class RequestResponseLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);
            await next(context);
            _logger.LogInformation("Response: {StatusCode}", context.Response.StatusCode);
        }
    }
}