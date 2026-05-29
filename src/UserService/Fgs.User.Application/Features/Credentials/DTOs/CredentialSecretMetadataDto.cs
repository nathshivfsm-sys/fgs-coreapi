namespace Fgs.User.Application.Features.Credentials.DTOs;

public sealed class CredentialSecretMetadataDto
{
    public Guid SecretId { get; init; }

    public Guid ProviderId { get; init; }

    public string ProviderCode { get; init; } = null!;

    public string ProviderName { get; init; } = null!;

    public string SecretName { get; init; } = null!;

    public string AwsSecretArn { get; init; } = null!;

    public string RegionName { get; init; } = null!;

    public int VersionNo { get; init; }

    public bool IsActive { get; init; }

    public bool IsRevoked { get; init; }

    public DateTimeOffset? LastRotatedOn { get; init; }

    public DateTimeOffset? ExpiresOn { get; init; }

    public DateTimeOffset CreatedOn { get; init; }

    public DateTimeOffset? UpdatedOn { get; init; }
}
