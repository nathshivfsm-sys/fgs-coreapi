using System.Net;
using FluentValidation;
using Fgs.Foundation.Constants;

namespace Fgs.Foundation.Middleware;

internal static class ExceptionMappingRules
{
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
            _ => (
                HttpStatusCode.InternalServerError,
                [ApiErrorMessages.UnexpectedError])
        };
}
