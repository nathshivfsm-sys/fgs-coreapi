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
        string codeChallenge,
        string? loginHint = null,
        bool forceSignup = false,
        string? userFlow = null) =>
        BuildAuthorizeUrl(state, redirectUri, loginHint, codeChallenge, forceSignup, userFlow);

    public string BuildLoginAuthorizationUrl(
        string state,
        string redirectUri,
        string codeChallenge,
        string? loginHint = null,
        string? userFlow = null) =>
        BuildAuthorizeUrl(state, redirectUri, loginHint, codeChallenge, forceSignup: false, userFlow);

    public Task<EntraTokenResult> ExchangeLoginCodeAsync(
        string code,
        string redirectUri,
        string codeVerifier,
        CancellationToken cancellationToken = default,
        string? userFlow = null) =>
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
            ResolveUserFlow(userFlow),
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
            ResolveUserFlow(null),
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
        string? userFlow,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await entraOAuthClient.ExchangeAuthorizationCodeAsync(
                form,
                string.IsNullOrWhiteSpace(userFlow) ? null : userFlow,
                cancellationToken);

            var accessToken = response.Access_token
                ?? throw new InvalidOperationException("access_token missing from Entra response.");

            // CIAM access tokens often omit email; id_token carries profile claims.
            var (objectId, email, displayName) = ParseUserClaims(accessToken, response.Id_token);

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
        catch (Refit.ApiException ex)
        {
            var detail = string.IsNullOrWhiteSpace(ex.Content) ? ex.Message : ex.Content.Trim();
            if (detail.Length > 800)
            {
                detail = detail[..800] + "…";
            }

            throw new InvalidOperationException(
                $"Entra token HTTP {(int)ex.StatusCode}: {detail}",
                ex);
        }
    }

    private string? ResolveUserFlow(string? userFlow) =>
        !string.IsNullOrWhiteSpace(userFlow)
            ? userFlow
            : (string.IsNullOrWhiteSpace(_options.UserFlow) ? null : _options.UserFlow);

    internal static (string ObjectId, string Email, string? DisplayName) ParseUserClaims(
        string accessToken,
        string? idToken)
    {
        var fromAccess = TryReadClaims(accessToken);
        var fromId = string.IsNullOrWhiteSpace(idToken) ? default : TryReadClaims(idToken);

        var oid = FirstNonEmpty(fromAccess.ObjectId, fromId.ObjectId);
        var email = FirstNonEmpty(fromAccess.Email, fromId.Email);
        var name = FirstNonEmpty(fromAccess.DisplayName, fromId.DisplayName);

        if (string.IsNullOrWhiteSpace(oid) || string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException(
                "Required Entra claims (oid, email) were not present on access_token or id_token.");
        }

        return (oid, email, name);
    }

    private static (string? ObjectId, string? Email, string? DisplayName) TryReadClaims(string jwt)
    {
        var parts = jwt.Split('.');
        if (parts.Length < 2)
        {
            return default;
        }

        try
        {
            var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(parts[1]));
            using var doc = JsonDocument.Parse(payloadJson);
            var root = doc.RootElement;

            var oid = root.TryGetProperty("oid", out var oidEl) ? oidEl.GetString()
                : root.TryGetProperty("sub", out var subEl) ? subEl.GetString()
                : null;

            var email = root.TryGetProperty("email", out var emailEl) ? emailEl.GetString()
                : root.TryGetProperty("preferred_username", out var prefEl) ? prefEl.GetString()
                : TryReadEmailsArray(root);

            var name = root.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null;

            return (oid, email, name);
        }
        catch (Exception)
        {
            return default;
        }
    }

    private static string? TryReadEmailsArray(JsonElement root)
    {
        if (!root.TryGetProperty("emails", out var emails) || emails.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        foreach (var item in emails.EnumerateArray())
        {
            if (item.ValueKind == JsonValueKind.String)
            {
                var value = item.GetString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }
        }

        return null;
    }

    private static string? FirstNonEmpty(params string?[] values) =>
        values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));

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
