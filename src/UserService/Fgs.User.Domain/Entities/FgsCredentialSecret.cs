namespace Fgs.User.Domain.Entities;

public class FgsCredentialSecret : FgsEntityBase
{
    public Guid Id { get; set; }

    public Guid CredentialProviderId { get; set; }

    public string VaultProvider { get; set; } = null!;

    public string SecretName { get; set; } = null!;

    public string? SecretArn { get; set; }

    public string RegionName { get; set; } = null!;

    public string? KmsKeyArn { get; set; }

    public bool RotationEnabled { get; set; } = true;

    public int VersionNo { get; set; } = 1;

    public DateTimeOffset? RotatedOn { get; set; }

    public DateTimeOffset? LastValidatedOn { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; } = true;
}
