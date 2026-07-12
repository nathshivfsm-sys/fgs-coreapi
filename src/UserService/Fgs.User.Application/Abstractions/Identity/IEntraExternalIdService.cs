namespace Fgs.User.Application.Abstractions.Identity;

public interface IEntraExternalIdService
{
    string BuildAuthorizationUrl(string state, string redirectUri, string? loginHint = null);

    Task<EntraTokenResult> ExchangeCodeAsync(string code, string redirectUri, CancellationToken cancellationToken = default);
}

public sealed record EntraTokenResult(
    string AccessToken,
    string ObjectId,
    string Email,
    string? DisplayName);
