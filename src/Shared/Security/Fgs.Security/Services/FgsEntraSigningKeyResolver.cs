using Fgs.Security.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Fgs.Security.Services;

public sealed class FgsEntraSigningKeyResolver(EntraExternalIdAuthOptions options)
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private IReadOnlyList<SecurityKey> _signingKeys = Array.Empty<SecurityKey>();
    private DateTimeOffset _refreshAfter = DateTimeOffset.MinValue;

    public IEnumerable<SecurityKey> Resolve(
        string token,
        SecurityToken? securityToken,
        string? kid,
        TokenValidationParameters validationParameters)
    {
        EnsureKeysLoaded();

        if (string.IsNullOrEmpty(kid))
        {
            return _signingKeys;
        }

        var matched = _signingKeys
            .Where(key => string.Equals(key.KeyId, kid, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return matched.Count > 0 ? matched : _signingKeys;
    }

    private void EnsureKeysLoaded()
    {
        if (_signingKeys.Count > 0 && DateTimeOffset.UtcNow < _refreshAfter)
        {
            return;
        }

        _lock.Wait();
        try
        {
            if (_signingKeys.Count > 0 && DateTimeOffset.UtcNow < _refreshAfter)
            {
                return;
            }

            _signingKeys = LoadSigningKeysAsync(CancellationToken.None).GetAwaiter().GetResult();
            _refreshAfter = DateTimeOffset.UtcNow.AddHours(6);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IReadOnlyList<SecurityKey>> LoadSigningKeysAsync(CancellationToken cancellationToken)
    {
        var tenantId = options.TenantId.Trim('/');
        var metadataAddresses = new[]
        {
            options.ResolveMetadataAddress(),
            $"https://login.microsoftonline.com/{tenantId}/v2.0/.well-known/openid-configuration",
            $"https://{tenantId}.ciamlogin.com/{tenantId}/v2.0/.well-known/openid-configuration"
        };

        var signingKeys = new List<SecurityKey>();
        foreach (var metadataAddress in metadataAddresses.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            try
            {
                var manager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    metadataAddress,
                    new OpenIdConnectConfigurationRetriever());
                var configuration = await manager.GetConfigurationAsync(cancellationToken);
                signingKeys.AddRange(configuration.SigningKeys);
            }
            catch
            {
                // Best-effort: continue with other metadata endpoints.
            }
        }

        return signingKeys
            .GroupBy(key => key.KeyId, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .ToList();
    }
}
