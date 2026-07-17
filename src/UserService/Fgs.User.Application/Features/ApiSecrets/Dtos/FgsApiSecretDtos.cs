namespace Fgs.User.Application.Features.ApiSecrets.Dtos;

public sealed record FgsApiSecretSummaryDto(
    long Id,
    long FgsApiClientId,
    string Name,
    DateTimeOffset? ExpiresOn,
    DateTimeOffset? LastUsedOn,
    DateTimeOffset? RevokedOn,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsApiSecretDetailDto(
    long Id,
    long FgsApiClientId,
    string Name,
    DateTimeOffset? ExpiresOn,
    DateTimeOffset? LastUsedOn,
    DateTimeOffset? RevokedOn,
    string? RevokedBy,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsApiSecretCreateResultDto(
    long Id,
    long FgsApiClientId,
    string Name,
    string Secret,
    DateTimeOffset? ExpiresOn,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string CreatedBy);

public sealed record FgsApiSecretCreateDto(
    long FgsApiClientId,
    string Name,
    DateTimeOffset? ExpiresOn);

public sealed record FgsApiSecretPatchDto(
    string? Name,
    DateTimeOffset? ExpiresOn,
    bool? IsActive);

public sealed record FgsApiSecretListFilters(
    long? FgsApiClientId = null);
