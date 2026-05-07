namespace UserService.Application.Common.Models;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }
    public int StatusCode { get; init; }
    public T? Data { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();

    public static ApiResponse<T> Ok(T data, int statusCode = 200) =>
        new()
        {
            Success = true,
            StatusCode = statusCode,
            Data = data,
            Errors = Array.Empty<string>()
        };

    public static ApiResponse<T> Fail(int statusCode, IReadOnlyList<string> errors) =>
        new()
        {
            Success = false,
            StatusCode = statusCode,
            Data = default,
            Errors = errors.Count == 0 ? new[] { "An error occurred." } : errors
        };

    public static ApiResponse<T> Fail(int statusCode, params string[] errors) =>
        Fail(statusCode, (IReadOnlyList<string>)errors);
}
