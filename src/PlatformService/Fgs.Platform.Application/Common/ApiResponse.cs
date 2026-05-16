namespace Fgs.Platform.Application.Common;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }

    public int StatusCode { get; init; }

    public T? Data { get; init; }

    public IReadOnlyList<string> Errors { get; init; } = [];

    public static ApiResponse<T> Ok(T data, int statusCode = ApiStatusCodes.Ok) =>
        new()
        {
            Success = true,
            StatusCode = statusCode,
            Data = data,
            Errors = []
        };

    public static ApiResponse<T> Fail(IEnumerable<string> errors, int statusCode) =>
        new()
        {
            Success = false,
            StatusCode = statusCode,
            Data = default,
            Errors = errors.ToList()
        };
}

public static class ApiStatusCodes
{
    public const int Ok = 200;
    public const int BadRequest = 400;
    public const int NotFound = 404;
    public const int InternalServerError = 500;
}
