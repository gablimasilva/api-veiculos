using WebApi.Middleware;

namespace WebApi.Extensions;

public static class MiddlewareExtension
{
    public static IApplicationBuilder UseGlobalExceptionHandler(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}