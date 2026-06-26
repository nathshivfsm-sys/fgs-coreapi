namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Platform-owned credential encrypted using AWS KMS envelope encryption.
/// </summary>
public class GloCredential
{
    public int Id { get; set; }

    public int CredentialProviderTypeId { get; set; }

    public string CredentialName { get; set; } = null!;

    public string? Description { get; set; }

    public byte[] CredentialData { get; set; } = null!;

    public byte[] EncryptedDataKey { get; set; } = null!;

    public string? KeyIdentifier { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public GloCredentialProviderType ProviderType { get; set; } = null!;
}
