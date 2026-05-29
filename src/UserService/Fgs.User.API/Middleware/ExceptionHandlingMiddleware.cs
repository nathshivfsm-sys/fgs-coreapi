using System.Net;
using System.Text.Json;
using FluentValidation;
using Fgs.User.API.Constants;
using Fgs.User.Application.Common;
using Fgs.User.Application.Credentials;

namespace Fgs.User.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
        var (statusCode, errors) = exception switch
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
            CredentialSecretsException vaultEx => (
                vaultEx.IsAccessDenied ? HttpStatusCode.Forbidden : HttpStatusCode.BadGateway,
                new[] { vaultEx.Message }),
            _ => (
                HttpStatusCode.InternalServerError,
                new[] { ApiErrorMessages.UnexpectedError })
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception (CorrelationId={CorrelationId})", context.TraceIdentifier);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse<object>.Fail(errors, (int)statusCode);
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
