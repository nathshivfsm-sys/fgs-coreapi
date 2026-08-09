namespace Fgs.Contracts.Api;

public static class ApiResponseExtensions
{
    public static T EnsureSuccess<T>(this ApiResponse<T> response)
    {
        if (response.Success && response.Data is not null)
        {
            return response.Data;
        }

        var message = response.Errors.Count > 0
            ? string.Join("; ", response.Errors)
            : $"Request failed with status code {response.StatusCode}.";

        throw response.StatusCode switch
        {
            ApiStatusCodes.NotFound => new KeyNotFoundException(message),
            ApiStatusCodes.Unauthorized => new UnauthorizedAccessException(message),
            ApiStatusCodes.Forbidden => new UnauthorizedAccessException(message),
            _ => new InvalidOperationException(message)
        };
    }

    public static void ThrowIfFailed<T>(this ApiResponse<T> response) => _ = response.EnsureSuccess();
}
