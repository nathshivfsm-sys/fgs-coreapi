namespace Fgs.Setup.Application.Common.Options;

/// <summary>
/// Flattened decrypted credential values bound to configuration at startup.
/// Keys follow <c>Global:{ProviderCode}:{Property}</c> or <c>Tenant:{TenantId}:{CompanyId}:{ProviderCode}:{Property}</c>.
/// </summary>
public sealed class CredentialConfigurationOptions
{
    public const string SectionName = "ResolvedCredentials";

    public Dictionary<string, string> Values { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
