using BookingService.API.Middlewares;

namespace BookingService.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            return app;
        }
    }
}
