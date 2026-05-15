using System.Text;
using System.Text.Json;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure.Identity;

public sealed class EntraExternalIdService : IEntraExternalIdService
{
    private readonly EntraExternalIdOptions _options;
    private readonly HttpClient _httpClient;

    public EntraExternalIdService(IOptions<EntraExternalIdOptions> options, HttpClient httpClient)
    {
        _options = options.Value;
        _httpClient = httpClient;
    }

    public string BuildAuthorizationUrl(Guid invitationId, string redirectUri)
    {
        var authorize = string.IsNullOrWhiteSpace(_options.AuthorizeEndpoint)
            ? $"{_options.Authority.TrimEnd('/')}/{_options.TenantId}/oauth2/v2.0/authorize"
            : _options.AuthorizeEndpoint;

        var query = new Dictionary<string, string?>
        {
            ["client_id"] = _options.ClientId,
            ["response_type"] = "code",
            ["redirect_uri"] = redirectUri,
            ["response_mode"] = "query",
            ["scope"] = _options.Scopes,
            ["state"] = invitationId.ToString()
        };

        var qs = string.Join("&", query
            .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
            .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value!)}"));

        return $"{authorize}?{qs}";
    }

    public async Task<EntraTokenResult> ExchangeCodeAsync(
        string code,
        string redirectUri,
        CancellationToken cancellationToken = default)
    {
        var tokenEndpoint = string.IsNullOrWhiteSpace(_options.TokenEndpoint)
            ? $"{_options.Authority.TrimEnd('/')}/{_options.TenantId}/oauth2/v2.0/token"
            : _options.TokenEndpoint;

        using var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = _options.ClientId,
            ["client_secret"] = _options.ClientSecret,
            ["code"] = code,
            ["redirect_uri"] = redirectUri,
            ["scope"] = _options.Scopes
        });

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
        var root = document.RootElement;

        var accessToken = root.GetProperty("access_token").GetString()
            ?? throw new InvalidOperationException("access_token missing from Entra response.");

        return ParseUserFromAccessToken(accessToken);
    }

    private static EntraTokenResult ParseUserFromAccessToken(string accessToken)
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

        return new EntraTokenResult(oid, email, name);
    }

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
