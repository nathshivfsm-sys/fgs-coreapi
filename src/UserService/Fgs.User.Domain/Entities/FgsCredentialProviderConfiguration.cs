namespace Fgs.User.Domain.Entities;

public class FgsCredentialProviderConfiguration : FgsEntityBase
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public long CompanyId { get; set; }

    public Guid CredentialProviderId { get; set; }

    public string ConfigurationKey { get; set; } = null!;

    public string? ConfigurationValue { get; set; }

    public string? Environment { get; set; }

    public bool IsActive { get; set; } = true;
}
