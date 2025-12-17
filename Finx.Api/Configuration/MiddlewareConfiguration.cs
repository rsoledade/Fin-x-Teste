using Finx.Api.Middleware;

namespace Finx.Api.Configuration
{
    public static class MiddlewareConfiguration
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            // Add validation exception middleware early in pipeline
            app.UseMiddleware<ValidationExceptionMiddleware>();

            return app;
        }
    }
}
