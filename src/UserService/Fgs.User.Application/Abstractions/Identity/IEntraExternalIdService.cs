namespace Fgs.User.Application.Abstractions.Identity;

public interface IEntraExternalIdService
{
    /// <summary>
    /// Builds the authorize URL for signup/invite (API callback redirect URI, no PKCE).
    /// </summary>
    string BuildAuthorizationUrl(string state, string redirectUri, string? loginHint = null);

    /// <summary>
    /// Builds the authorize URL for returning-user login (SPA redirect URI + PKCE S256).
    /// </summary>
    string BuildLoginAuthorizationUrl(
        string state,
        string redirectUri,
        string codeChallenge,
        string? loginHint = null);

    /// <summary>
    /// Exchanges an authorization code for signup/invite (no PKCE verifier).
    /// </summary>
    Task<EntraTokenResult> ExchangeCodeAsync(
        string code,
        string redirectUri,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Exchanges an authorization code for returning-user login (PKCE verifier required).
    /// </summary>
    Task<EntraTokenResult> ExchangeLoginCodeAsync(
        string code,
        string redirectUri,
        string codeVerifier,
        CancellationToken cancellationToken = default);

    Task<EntraTokenResult> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);
}

public sealed record EntraTokenResult(
    string AccessToken,
    string ObjectId,
    string Email,
    string? DisplayName,
    string? RefreshToken = null,
    string? IdToken = null,
    int ExpiresIn = 3600,
    string TokenType = "Bearer");
