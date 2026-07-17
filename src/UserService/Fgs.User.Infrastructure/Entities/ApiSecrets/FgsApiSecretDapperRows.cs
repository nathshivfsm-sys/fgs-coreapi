using Fgs.User.Application.Features.ApiSecrets.Dtos;

namespace Fgs.User.Infrastructure.Entities.ApiSecrets;

internal sealed class FgsApiSecretSummaryRow
{
    public long Id { get; set; }

    public long FgsApiClientId { get; set; }

    public string Name { get; set; } = null!;

    public DateTimeOffset? ExpiresOn { get; set; }

    public DateTimeOffset? LastUsedOn { get; set; }

    public DateTimeOffset? RevokedOn { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public FgsApiSecretSummaryDto ToDto() =>
        new(
            Id,
            FgsApiClientId,
            Name,
            ExpiresOn,
            LastUsedOn,
            RevokedOn,
            IsActive,
            CreatedOn,
            CreatedBy);
}

internal sealed class FgsApiSecretDetailRow
{
    public long Id { get; set; }

    public long FgsApiClientId { get; set; }

    public string Name { get; set; } = null!;

    public DateTimeOffset? ExpiresOn { get; set; }

    public DateTimeOffset? LastUsedOn { get; set; }

    public DateTimeOffset? RevokedOn { get; set; }

    public string? RevokedBy { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public FgsApiSecretDetailDto ToDto() =>
        new(
            Id,
            FgsApiClientId,
            Name,
            ExpiresOn,
            LastUsedOn,
            RevokedOn,
            RevokedBy,
            IsActive,
            CreatedOn,
            CreatedBy);
}
