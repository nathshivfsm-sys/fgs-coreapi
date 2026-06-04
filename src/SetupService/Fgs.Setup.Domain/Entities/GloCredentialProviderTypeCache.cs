namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Local cache of <see cref="GloCredentialProviderType"/> to avoid cross-schema FKs from setup to glo.
/// </summary>
public class GloCredentialProviderTypeCache
{
    public int ProviderTypeId { get; set; }

    public string ProviderCode { get; set; } = null!;

    public string ProviderName { get; set; } = null!;

    public string ConfigurationSchema { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset? UpdatedOn { get; set; }
}
