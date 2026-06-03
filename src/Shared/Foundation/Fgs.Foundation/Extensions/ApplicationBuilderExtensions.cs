using Fgs.Foundation.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Fgs.Foundation.Extensions;

public sealed class FgsFoundationMiddlewareOptions
{
    public Func<PathString, bool>? OmitRequestBodyLoggingForPath { get; set; }

    public bool UseSecurityHeaders { get; set; } = true;

    public bool UseRequestResponseLogging { get; set; } = true;

    public bool UseExceptionHandling { get; set; } = true;
}

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseFgsFoundationMiddleware(
        this IApplicationBuilder app,
        Action<FgsFoundationMiddlewareOptions>? configure = null)
    {
        var options = new FgsFoundationMiddlewareOptions();
        configure?.Invoke(options);

        app.UseMiddleware<CorrelationIdMiddleware>();

        if (options.UseSecurityHeaders)
        {
            app.UseMiddleware<SecurityHeadersMiddleware>();
        }

        if (options.UseRequestResponseLogging)
        {
            if (options.OmitRequestBodyLoggingForPath is { } omitBodyForPath)
            {
                app.UseMiddleware<RequestResponseLoggingMiddleware>(omitBodyForPath);
            }
            else
            {
                app.UseMiddleware<RequestResponseLoggingMiddleware>();
            }
        }

        if (options.UseExceptionHandling)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        return app;
    }
}
