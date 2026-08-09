using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fgs.Foundation.Correlation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fgs.Foundation.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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
        var title = ReasonPhrases.GetReasonPhrase((int)statusCode);
        if (string.IsNullOrWhiteSpace(title))
        {
            title = statusCode.ToString();
        }

        var detail = errors.Count > 0 ? errors[0] : title;
        var problem = new ApiProblemDetails
        {
            Type = $"https://httpstatuses.com/{(int)statusCode}",
            Title = title,
            Status = (int)statusCode,
            Detail = detail,
            Instance = context.Request.Path.HasValue ? context.Request.Path.Value : null,
            TraceId = context.TraceIdentifier,
            CorrelationId = correlationId,
            StatusCode = (int)statusCode,
            Errors = errors,
            Success = false,
            Data = null
        };

        context.Response.Clear();
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;
        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            context.Response.Headers["X-Correlation-ID"] = correlationId;
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem, SerializerOptions));
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

/// <summary>
/// RFC7807-aligned problem payload that also preserves FGS API envelope field names
/// (<c>success</c>, <c>statusCode</c>, <c>errors</c>, <c>data</c>) for existing clients.
/// </summary>
public sealed class ApiProblemDetails
{
    public string? Type { get; init; }

    public string? Title { get; init; }

    public int Status { get; init; }

    public string? Detail { get; init; }

    public string? Instance { get; init; }

    public string? TraceId { get; init; }

    public string? CorrelationId { get; init; }

    public bool Success { get; init; }

    public int StatusCode { get; init; }

    public object? Data { get; init; }

    public IReadOnlyList<string> Errors { get; init; } = [];
}

internal static class ReasonPhrases
{
    public static string GetReasonPhrase(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        409 => "Conflict",
        422 => "Unprocessable Entity",
        500 => "Internal Server Error",
        502 => "Bad Gateway",
        503 => "Service Unavailable",
        _ => string.Empty
    };
}
