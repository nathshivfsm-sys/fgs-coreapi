using System.Net;
using System.Text.Json;
using FluentValidation;
using Fgs.Foundation.Constants;
using Fgs.Contracts.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fgs.Foundation.Middleware;

public sealed class ExceptionHandlingMiddleware
{
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
                "Unhandled exception (CorrelationId={CorrelationId})",
                context.TraceIdentifier);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse<object>.Fail(errors, (int)statusCode);
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
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

        return exception switch
        {
            ValidationException validation => (
                HttpStatusCode.BadRequest,
                validation.Errors.Select(e => e.ErrorMessage).ToArray()),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                new[] { ApiErrorMessages.Unauthorized }),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                new[] { exception.Message }),
            _ => (
                HttpStatusCode.InternalServerError,
                new[] { ApiErrorMessages.UnexpectedError })
        };
    }
}
