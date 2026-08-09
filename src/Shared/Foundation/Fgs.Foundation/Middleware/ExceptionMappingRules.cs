using System.Net;
using System.Text.Json;
using FluentValidation;
using Fgs.Foundation.Constants;
using Refit;

namespace Fgs.Foundation.Middleware;

internal static class ExceptionMappingRules
{
    private static readonly JsonSerializerOptions ApiResponseJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static (HttpStatusCode StatusCode, IReadOnlyList<string> Errors) Map(Exception exception) =>
        exception switch
        {
            ValidationException validation => (
                HttpStatusCode.BadRequest,
                validation.Errors
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage)
                        ? $"{e.PropertyName} is invalid."
                        : e.ErrorMessage)
                    .ToArray()),
            KeyNotFoundException notFound => (
                HttpStatusCode.NotFound,
                [ResolveMessage(notFound)]),
            InvalidOperationException invalidOperation => (
                invalidOperation.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    ? HttpStatusCode.NotFound
                    : HttpStatusCode.Conflict,
                [ResolveMessage(invalidOperation)]),
            ArgumentException argument => (
                HttpStatusCode.BadRequest,
                [ResolveMessage(argument)]),
            FormatException format => (
                HttpStatusCode.BadRequest,
                [ResolveMessage(format)]),
            UnauthorizedAccessException unauthorized => (
                HttpStatusCode.Unauthorized,
                [ResolveMessage(unauthorized, ApiErrorMessages.Unauthorized)]),
            ApiException apiException => MapApiException(apiException),
            _ => (
                MapUnknownStatus(exception),
                [GetDetailedMessage(exception)])
        };

    private static HttpStatusCode MapUnknownStatus(Exception exception)
    {
        // EF Core / Npgsql type names without taking a package dependency.
        var typeName = exception.GetType().FullName ?? exception.GetType().Name;
        if (typeName.Contains("DbUpdateException", StringComparison.Ordinal)
            || typeName.Contains("PostgresException", StringComparison.Ordinal)
            || typeName.Contains("NpgsqlException", StringComparison.Ordinal))
        {
            return HttpStatusCode.Conflict;
        }

        return HttpStatusCode.InternalServerError;
    }

    private static string ResolveMessage(Exception exception, string? fallback = null)
    {
        if (!string.IsNullOrWhiteSpace(exception.Message))
        {
            return exception.Message;
        }

        return GetDetailedMessage(exception, fallback);
    }

    private static string GetDetailedMessage(Exception exception, string? fallback = null)
    {
        var current = exception;
        while (current.InnerException is not null)
        {
            // Keep descriptive wrapper messages (e.g. FluentValidation) instead of a bare NRE.
            if (IsGenericNullReferenceMessage(current.InnerException)
                && !string.IsNullOrWhiteSpace(current.Message)
                && !IsGenericNullReferenceMessage(current))
            {
                break;
            }

            current = current.InnerException;
        }

        if (!string.IsNullOrWhiteSpace(current.Message))
        {
            return current.Message;
        }

        if (!string.IsNullOrWhiteSpace(exception.Message))
        {
            return exception.Message;
        }

        if (!string.IsNullOrWhiteSpace(fallback))
        {
            return fallback;
        }

        return $"{exception.GetType().Name}: {ApiErrorMessages.UnexpectedError}";
    }

    private static bool IsGenericNullReferenceMessage(Exception exception) =>
        exception is NullReferenceException
        && (string.IsNullOrWhiteSpace(exception.Message)
            || string.Equals(
                exception.Message,
                "Object reference not set to an instance of an object.",
                StringComparison.Ordinal));

    private static (HttpStatusCode StatusCode, IReadOnlyList<string> Errors) MapApiException(
        ApiException apiException)
    {
        if (!string.IsNullOrWhiteSpace(apiException.Content))
        {
            try
            {
                var payload = JsonSerializer.Deserialize<Fgs.Contracts.Api.ApiResponse<object>>(
                    apiException.Content,
                    ApiResponseJsonOptions);

                if (payload?.Errors is { Count: > 0 })
                {
                    var statusCode = payload.StatusCode is > 0
                        ? (HttpStatusCode)payload.StatusCode
                        : apiException.StatusCode;
                    return (statusCode, payload.Errors);
                }
            }
            catch (JsonException)
            {
                // Fall through to HTTP status / detailed message.
            }
        }

        if (!string.IsNullOrWhiteSpace(apiException.Content)
            && apiException.StatusCode is >= HttpStatusCode.BadRequest and < HttpStatusCode.InternalServerError)
        {
            return (apiException.StatusCode, [apiException.Content.Trim()]);
        }

        var message = GetDetailedMessage(apiException);
        return apiException.StatusCode is >= HttpStatusCode.BadRequest and < HttpStatusCode.InternalServerError
            ? (apiException.StatusCode, [message])
            : (HttpStatusCode.InternalServerError, [message]);
    }
}
