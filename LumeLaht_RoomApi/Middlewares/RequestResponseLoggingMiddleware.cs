namespace LumeLaht_RoomApi.Middlewares
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
            // Logging Request
            context.Request.EnableBuffering(); // check body once

            var requestBody = await ReadRequestBody(context.Request);
            _logger.LogInformation("HTTP Request Information: {Method} {Path} {Body}",
                context.Request.Method,
                context.Request.Path,
                requestBody);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next(context); // moving along the pipeline 

            // Logging Response
            var responseText = await ReadResponseBody(context.Response);
            _logger.LogInformation("HTTP Response Information: {StatusCode} {Body}",
                context.Response.StatusCode,
                responseText);

            // return response to client
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin); //Return to the beginning
            return body;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }

}
