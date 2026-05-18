namespace Fgs.User.Domain.Entities;

public class FgsCredentialSecret : FgsEntityBase
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public long CompanyId { get; set; }

    public Guid CredentialProviderId { get; set; }

    public string SecretName { get; set; } = null!;

    public string EncryptedSecretValue { get; set; } = null!;

    public string EncryptedDek { get; set; } = null!;

    public string EncryptionKeyId { get; set; } = null!;

    public int VersionNo { get; set; } = 1;

    public DateTimeOffset? LastRotatedOn { get; set; }

    public DateTimeOffset? ExpiresOn { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsRevoked { get; set; }
}
