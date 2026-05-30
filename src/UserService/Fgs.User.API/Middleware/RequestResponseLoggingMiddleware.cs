using System.Diagnostics;

namespace Fgs.User.API.Middleware;

public sealed class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    private static bool IsCredentialRoute(PathString path) =>
        path.StartsWithSegments("/api/credentials", StringComparison.OrdinalIgnoreCase);

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.TraceIdentifier;
        var sw = Stopwatch.StartNew();
        var omitBody = IsCredentialRoute(context.Request.Path);

        if (omitBody)
        {
            _logger.LogInformation(
                "HTTP {Method} {Path} started (CorrelationId={CorrelationId}, body omitted)",
                context.Request.Method,
                context.Request.Path,
                correlationId);
        }
        else
        {
            _logger.LogInformation(
                "HTTP {Method} {Path} started (CorrelationId={CorrelationId})",
                context.Request.Method,
                context.Request.Path,
                correlationId);
        }

        await _next(context);

        sw.Stop();
        _logger.LogInformation(
            "HTTP {Method} {Path} completed {StatusCode} in {ElapsedMs}ms (CorrelationId={CorrelationId})",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            sw.ElapsedMilliseconds,
            correlationId);
    }
}
