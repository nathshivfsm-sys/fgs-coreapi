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
                validation.Errors.Select(e => e.ErrorMessage).ToArray()),
            KeyNotFoundException notFound => (
                HttpStatusCode.NotFound,
                [notFound.Message]),
            InvalidOperationException invalidOperation => (
                invalidOperation.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    ? HttpStatusCode.NotFound
                    : HttpStatusCode.Conflict,
                [invalidOperation.Message]),
            ArgumentException argument => (
                HttpStatusCode.BadRequest,
                [argument.Message]),
            FormatException format => (
                HttpStatusCode.BadRequest,
                [format.Message]),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                [ApiErrorMessages.Unauthorized]),
            ApiException apiException => MapApiException(apiException),
            _ => (
                HttpStatusCode.InternalServerError,
                [ApiErrorMessages.UnexpectedError])
        };

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
                // Fall through to HTTP status / generic message.
            }
        }

        if (!string.IsNullOrWhiteSpace(apiException.Content)
            && apiException.StatusCode is >= HttpStatusCode.BadRequest and < HttpStatusCode.InternalServerError)
        {
            return (apiException.StatusCode, [apiException.Content.Trim()]);
        }

        return apiException.StatusCode is >= HttpStatusCode.BadRequest and < HttpStatusCode.InternalServerError
            ? (apiException.StatusCode, [apiException.Message])
            : (HttpStatusCode.InternalServerError, [ApiErrorMessages.UnexpectedError]);
    }
}
