namespace Fgs.Security.Authorization;

public sealed record UserAuthorizationResult(
    bool Success,
    int? StatusCode = null,
    string? ErrorMessage = null,
    ValidatedUserScope? ValidatedScope = null)
{
    public static UserAuthorizationResult Ok(ValidatedUserScope? validatedScope = null) =>
        new(true, ValidatedScope: validatedScope);

    public static UserAuthorizationResult Fail(int statusCode, string errorMessage) =>
        new(false, statusCode, errorMessage);
}
