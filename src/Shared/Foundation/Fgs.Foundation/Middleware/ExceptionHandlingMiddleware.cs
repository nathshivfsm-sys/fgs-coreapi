using System.Net;
using System.Text.Json;
using Fgs.Contracts.Api;
using Fgs.Foundation.Correlation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fgs.Foundation.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IEnumerable<IExceptionStatusMapper> _mappers;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IEnumerable<IExceptionStatusMapper> mappers)
    {
        _next = next;
        _logger = logger;
        _mappers = mappers;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, errors) = MapException(exception);

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(
                exception,
                "Unhandled exception (CorrelationId={CorrelationId}, TraceId={TraceId})",
                ResolveCorrelationId(context),
                context.TraceIdentifier);
        }
        else
        {
            _logger.LogWarning(
                exception,
                "Handled exception status {StatusCode} (CorrelationId={CorrelationId}, TraceId={TraceId})",
                (int)statusCode,
                ResolveCorrelationId(context),
                context.TraceIdentifier);
        }

        if (context.Response.HasStarted)
        {
            throw exception;
        }

        var correlationId = ResolveCorrelationId(context);
        var payloadErrors = errors.Count > 0
            ? errors
            : ["An unexpected error occurred."];

        var payload = ApiResponse<object>.Fail(payloadErrors, (int)statusCode);

        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            context.Response.Headers["X-Correlation-ID"] = correlationId;
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload, SerializerOptions));
    }

    private (HttpStatusCode StatusCode, IReadOnlyList<string> Errors) MapException(Exception exception)
    {
        foreach (var mapper in _mappers)
        {
            if (mapper.TryMap(exception, out var mapping))
            {
                return (mapping.StatusCode, mapping.Errors);
            }
        }

        return ExceptionMappingRules.Map(exception);
    }

    private static string ResolveCorrelationId(HttpContext context)
    {
        var correlationContext = context.RequestServices.GetService<ICorrelationContext>();
        if (correlationContext is not null)
        {
            return correlationContext.GetCorrelationId().ToString("N");
        }

        return context.TraceIdentifier;
    }
}
