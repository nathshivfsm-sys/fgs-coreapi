namespace Fgs.User.Application.Features.Tenants.Dtos;

public sealed record TenantDetailDto(
    long Id,
    Guid TenantGuid,
    string Code,
    string Name,
    string? LegalName,
    string? Email,
    string? PhoneNumber,
    string? Website,
    string? DefaultCurrency,
    int? DefaultLanguageId,
    short FgsTenantStatusId,
    string? StorageBucketName,
    bool IsActive);

public sealed record TenantSummaryDto(
    long Id,
    Guid TenantGuid,
    string Code,
    string Name,
    short FgsTenantStatusId,
    bool IsActive);

public sealed record TenantUpdateDto(
    string Name,
    string? LegalName,
    string? Email,
    string? PhoneNumber,
    string? Website,
    string? DefaultCurrency,
    int? DefaultLanguageId,
    bool IsActive);

public sealed record TenantPatchDto(
    string? Name = null,
    string? LegalName = null,
    string? Email = null,
    string? PhoneNumber = null,
    string? Website = null,
    string? DefaultCurrency = null,
    int? DefaultLanguageId = null,
    bool? IsActive = null);
