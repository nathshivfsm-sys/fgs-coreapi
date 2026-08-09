using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fgs.Foundation.Middleware;

public sealed class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
    private readonly Func<PathString, bool>? _omitBodyForPath;

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger,
        Func<PathString, bool>? omitBodyForPath = null)
    {
        _next = next;
        _logger = logger;
        _omitBodyForPath = omitBodyForPath;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.TraceIdentifier;
        var sw = Stopwatch.StartNew();
        var omitBody = _omitBodyForPath?.Invoke(context.Request.Path) == true;

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
            "HTTP {Method} {RequestPath} completed {StatusCode} in {Duration}ms (CorrelationId={CorrelationId})",
            context.Request.Method,
            context.Request.Path.Value,
            context.Response.StatusCode,
            sw.ElapsedMilliseconds,
            correlationId);
    }
}
