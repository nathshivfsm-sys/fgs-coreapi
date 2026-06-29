namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;

public sealed record FgsSetupTaxAuthoritySummaryDto(
    long Id,
    string Code,
    string Name,
    string? RegionCode,
    bool IsExternalSystemRecord,
    decimal TaxPercent,
    string? Description,
    int UsageCount,
    bool IsActive);

public sealed record FgsSetupTaxAuthorityDetailDto(
    long Id,
    string Code,
    string Name,
    string? RegionCode,
    bool IsExternalSystemRecord,
    decimal TaxPercent,
    string? Description,
    int UsageCount,
    bool IsActive);

public sealed record FgsSetupTaxAuthorityLookupDto(
    long Id,
    string Code,
    string Name,
    decimal TaxPercent);

public sealed record FgsSetupTaxAuthorityCreateDto(
    string Code,
    string Name,
    string? RegionCode,
    bool IsExternalSystemRecord,
    decimal TaxPercent,
    string? Description);

public sealed record FgsSetupTaxAuthorityUpdateDto(
    string Code,
    string Name,
    string? RegionCode,
    bool IsExternalSystemRecord,
    decimal TaxPercent,
    string? Description);

public sealed record FgsSetupTaxAuthorityPatchDto(
    string? Code,
    string? Name,
    string? RegionCode,
    bool? IsExternalSystemRecord,
    decimal? TaxPercent,
    string? Description,
    bool? IsActive);

public sealed record FgsSetupTaxAuthorityListFilters(
    string? Code = null,
    string? Name = null);
