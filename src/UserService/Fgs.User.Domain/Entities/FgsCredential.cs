using Fgs.Kernel.Entities;

namespace Fgs.User.Domain.Entities;

/// <summary>
/// Stores tenant-owned credentials encrypted using AWS KMS envelope encryption.
/// </summary>
public class FgsCredential : FgsEntityBase, ITenantCompanyScoped
{
    public Guid Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public int CredentialProviderTypeId { get; set; }

    /// <summary>User friendly name displayed in tenant administration screens.</summary>
    public string CredentialName { get; set; } = null!;

    /// <summary>Optional description of the credential usage.</summary>
    public string? Description { get; set; }

    /// <summary>Provider credential JSON encrypted using a Data Encryption Key (DEK).</summary>
    public byte[] CredentialData { get; set; } = null!;

    /// <summary>Data Encryption Key encrypted using AWS KMS.</summary>
    public byte[] EncryptedDataKey { get; set; } = null!;

    /// <summary>AWS KMS key ARN or alias used to encrypt the Data Encryption Key.</summary>
    public string? KeyIdentifier { get; set; }

    /// <summary>Indicates whether the credential is active and available for use.</summary>
    public bool IsActive { get; set; } = true;

    public GloCredentialProviderType ProviderType { get; set; } = null!;
}
