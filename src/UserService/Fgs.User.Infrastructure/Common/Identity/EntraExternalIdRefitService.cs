using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Fgs.Contracts.Clients;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure.Common.Identity;

public sealed class EntraExternalIdRefitService(
    IOptions<EntraExternalIdOptions> options,
    IEntraOAuthClient entraOAuthClient) : IEntraExternalIdService
{
    private readonly EntraExternalIdOptions _options = options.Value;

    public string BuildAuthorizationUrl(
        string state,
        string redirectUri,
        string? loginHint = null,
        bool forceSignup = false,
        string? userFlow = null) =>
        BuildAuthorizeUrl(state, redirectUri, loginHint, codeChallenge: null, forceSignup, userFlow);

    public string BuildLoginAuthorizationUrl(
        string state,
        string redirectUri,
        string codeChallenge,
        string? loginHint = null,
        string? userFlow = null) =>
        BuildAuthorizeUrl(state, redirectUri, loginHint, codeChallenge, forceSignup: false, userFlow);

    public Task<EntraTokenResult> ExchangeCodeAsync(
        string code,
        string redirectUri,
        CancellationToken cancellationToken = default) =>
        ExchangeAsync(
            new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["code"] = code,
                ["redirect_uri"] = redirectUri,
                ["scope"] = _options.Scopes
            },
            cancellationToken);

    public Task<EntraTokenResult> ExchangeLoginCodeAsync(
        string code,
        string redirectUri,
        string codeVerifier,
        CancellationToken cancellationToken = default) =>
        ExchangeAsync(
            new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["code"] = code,
                ["redirect_uri"] = redirectUri,
                ["code_verifier"] = codeVerifier,
                ["scope"] = _options.Scopes
            },
            cancellationToken);

    public Task<EntraTokenResult> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default) =>
        ExchangeAsync(
            new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["refresh_token"] = refreshToken,
                ["scope"] = _options.Scopes
            },
            cancellationToken);

    public static (string CodeVerifier, string CodeChallenge) CreatePkcePair()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        var verifier = Base64UrlEncode(bytes);
        var challenge = Base64UrlEncode(SHA256.HashData(Encoding.ASCII.GetBytes(verifier)));
        return (verifier, challenge);
    }

    private string BuildAuthorizeUrl(
        string state,
        string redirectUri,
        string? loginHint,
        string? codeChallenge,
        bool forceSignup,
        string? userFlow)
    {
        var authorize = string.IsNullOrWhiteSpace(_options.AuthorizeEndpoint)
            ? BuildAuthorizeEndpoint()
            : _options.AuthorizeEndpoint;

        var resolvedFlow = !string.IsNullOrWhiteSpace(userFlow)
            ? userFlow
            : _options.UserFlow;

        var query = new Dictionary<string, string?>
        {
            ["client_id"] = _options.ClientId,
            ["response_type"] = "code",
            ["redirect_uri"] = redirectUri,
            ["response_mode"] = "query",
            ["scope"] = _options.Scopes,
            ["state"] = state,
            ["login_hint"] = loginHint,
            ["p"] = resolvedFlow,
            // Entra External ID SignUpSignIn: prompt=create opens signup instead of sign-in.
            ["prompt"] = forceSignup ? "create" : null
        };

        if (!string.IsNullOrWhiteSpace(codeChallenge))
        {
            query["code_challenge"] = codeChallenge;
            query["code_challenge_method"] = "S256";
        }

        var qs = string.Join("&", query
            .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
            .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value!)}"));

        return $"{authorize}?{qs}";
    }

    private async Task<EntraTokenResult> ExchangeAsync(
        Dictionary<string, string> form,
        CancellationToken cancellationToken)
    {
        var response = await entraOAuthClient.ExchangeAuthorizationCodeAsync(form, cancellationToken);

        var accessToken = response.Access_token
            ?? throw new InvalidOperationException("access_token missing from Entra response.");

        var (objectId, email, displayName) = ParseUserFromAccessToken(accessToken);

        return new EntraTokenResult(
            accessToken,
            objectId,
            email,
            displayName,
            response.Refresh_token,
            response.Id_token,
            response.Expires_in ?? 3600,
            string.IsNullOrWhiteSpace(response.Token_type) ? "Bearer" : response.Token_type);
    }

    private static (string ObjectId, string Email, string? DisplayName) ParseUserFromAccessToken(string accessToken)
    {
        var parts = accessToken.Split('.');
        if (parts.Length < 2)
        {
            throw new InvalidOperationException("Invalid JWT access token from Entra.");
        }

        var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(parts[1]));
        using var doc = JsonDocument.Parse(payloadJson);
        var root = doc.RootElement;

        var oid = root.TryGetProperty("oid", out var oidEl) ? oidEl.GetString()
            : root.TryGetProperty("sub", out var subEl) ? subEl.GetString()
            : null;

        var email = root.TryGetProperty("email", out var emailEl) ? emailEl.GetString()
            : root.TryGetProperty("preferred_username", out var prefEl) ? prefEl.GetString()
            : null;

        var name = root.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null;

        if (string.IsNullOrWhiteSpace(oid) || string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException("Required Entra claims (oid, email) were not present.");
        }

        return (oid, email, name);
    }

    private string BuildAuthorizeEndpoint()
    {
        var authority = _options.Authority.TrimEnd('/');
        var tenant = _options.TenantId.Trim('/');
        return $"{authority}/{tenant}/oauth2/v2.0/authorize";
    }

    private static string Base64UrlEncode(byte[] input) =>
        Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static byte[] Base64UrlDecode(string input)
    {
        var padded = input.Replace('-', '+').Replace('_', '/');
        switch (padded.Length % 4)
        {
            case 2: padded += "=="; break;
            case 3: padded += "="; break;
        }

        return Convert.FromBase64String(padded);
    }
}
