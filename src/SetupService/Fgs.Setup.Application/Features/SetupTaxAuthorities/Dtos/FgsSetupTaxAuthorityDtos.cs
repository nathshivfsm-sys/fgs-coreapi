namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;

public sealed record FgsSetupTaxAuthoritySummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string Code,
    string Name,
    string? RegionCode,
    bool IsExternalSystemRecord,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSetupTaxAuthorityDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string Code,
    string Name,
    string? RegionCode,
    bool IsExternalSystemRecord,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsSetupTaxAuthorityLookupDto(
    long Id,
    string Code,
    string Name);

public sealed record FgsSetupTaxAuthorityCreateDto(
    string Code,
    string Name,
    string? RegionCode,
    bool IsExternalSystemRecord,
    string? Description);

public sealed record FgsSetupTaxAuthorityUpdateDto(
    string Code,
    string Name,
    string? RegionCode,
    bool IsExternalSystemRecord,
    string? Description);

public sealed record FgsSetupTaxAuthorityPatchDto(
    string? Code,
    string? Name,
    string? RegionCode,
    bool? IsExternalSystemRecord,
    string? Description,
    bool? IsActive);

public sealed record FgsSetupTaxAuthorityListFilters(
    string? Code = null,
    string? Name = null);
