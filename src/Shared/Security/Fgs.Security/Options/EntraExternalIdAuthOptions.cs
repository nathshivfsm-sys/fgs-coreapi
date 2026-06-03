namespace Fgs.Security.Options;

/// <summary>
/// Microsoft Entra External ID settings for JWT bearer validation (no client secret).
/// </summary>
public sealed class EntraExternalIdAuthOptions
{
    public const string SectionName = "EntraExternalId";

    public string TenantId { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    /// <summary>CIAM login host, e.g. https://fsdemoapp.ciamlogin.com</summary>
    public string Authority { get; set; } = string.Empty;

    /// <summary>Optional override; defaults to {Authority}/{TenantId}/v2.0</summary>
    public string MetadataAddress { get; set; } = string.Empty;

    public string ResolveAuthority()
    {
        if (string.IsNullOrWhiteSpace(Authority) || string.IsNullOrWhiteSpace(TenantId))
        {
            throw new InvalidOperationException(
                $"{SectionName}:Authority and {SectionName}:TenantId are required.");
        }

        return $"{Authority.TrimEnd('/')}/{TenantId.Trim('/')}/v2.0";
    }

    public string ResolveMetadataAddress()
    {
        if (!string.IsNullOrWhiteSpace(MetadataAddress))
        {
            return MetadataAddress;
        }

        return $"{ResolveAuthority()}/.well-known/openid-configuration";
    }
}
