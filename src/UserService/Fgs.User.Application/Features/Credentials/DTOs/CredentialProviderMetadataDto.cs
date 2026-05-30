namespace Fgs.User.Application.Features.Credentials.DTOs;

public sealed class CredentialProviderMetadataDto
{
    public Guid ProviderId { get; init; }

    public string Code { get; init; } = null!;

    public string Name { get; init; } = null!;

    public string Environment { get; init; } = null!;

    public int ProviderTypeId { get; init; }

    public string? ProviderTypeCode { get; init; }

    public bool IsActive { get; init; }
}
