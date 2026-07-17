using Fgs.Contracts.Api;
using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Authorization;

public static class AuthorizationResponseWriter
{
    public static async Task WriteAsync(
        HttpContext context,
        int statusCode,
        string errorMessage,
        CancellationToken cancellationToken = default)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var apiStatusCode = statusCode switch
        {
            StatusCodes.Status400BadRequest => ApiStatusCodes.BadRequest,
            _ => ApiStatusCodes.Forbidden
        };

        await context.Response.WriteAsJsonAsync(
            ApiResponse<object>.Fail([errorMessage], apiStatusCode),
            cancellationToken);
    }
}
