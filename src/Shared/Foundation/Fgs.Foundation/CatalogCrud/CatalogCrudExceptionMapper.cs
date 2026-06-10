using Fgs.Contracts.Api;

namespace Fgs.Foundation.CatalogCrud;

public static class CatalogCrudExceptionMapper
{
    public static ApiResponse<T> MapException<T>(Exception exception) =>
        exception switch
        {
            KeyNotFoundException notFound => ApiResponse<T>.Fail([notFound.Message], ApiStatusCodes.NotFound),
            InvalidOperationException invalidOperation => ApiResponse<T>.Fail(
                [invalidOperation.Message],
                invalidOperation.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    ? ApiStatusCodes.NotFound
                    : ApiStatusCodes.Conflict),
            ArgumentException argument => ApiResponse<T>.Fail([argument.Message], ApiStatusCodes.BadRequest),
            FormatException format => ApiResponse<T>.Fail([format.Message], ApiStatusCodes.BadRequest),
            _ => ApiResponse<T>.Fail([exception.Message], ApiStatusCodes.InternalServerError)
        };
}
