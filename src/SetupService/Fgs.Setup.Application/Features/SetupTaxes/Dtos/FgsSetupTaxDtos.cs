namespace Fgs.Setup.Application.Features.SetupTaxes.Dtos;

public sealed record FgsSetupTaxSummaryDto(
    long Id,
    string TaxCode,
    string Name,
    bool ShowTaxDetail,
    string? Description,
    decimal TaxRate,
    bool IsActive);

public sealed record FgsSetupTaxLineDetailDto(
    long Id,
    long FgsSetupTaxAuthorityId,
    string TaxAuthorityCode,
    string TaxAuthorityName,
    decimal TaxPercent,
    DateOnly EffectiveFromDate,
    DateOnly? EffectiveToDate,
    bool IsActive);

public sealed record FgsSetupTaxDetailDto(
    long Id,
    string TaxCode,
    string Name,
    bool ShowTaxDetail,
    string? Description,
    decimal TaxRate,
    bool IsActive,
    IReadOnlyList<FgsSetupTaxLineDetailDto> TaxDetails);

public sealed record FgsSetupTaxLookupDto(
    long Id,
    string TaxCode,
    string Name,
    decimal TaxRate);

public sealed record FgsSetupTaxCreateDto(
    string TaxCode,
    string Name,
    bool IsExternalSystemRecord,
    string? ExternalSystemId,
    string? SyncToken,
    bool ShowTaxDetail,
    string? Description);

public sealed record FgsSetupTaxUpdateDto(
    string TaxCode,
    string Name,
    bool IsExternalSystemRecord,
    string? ExternalSystemId,
    string? SyncToken,
    bool ShowTaxDetail,
    string? Description);

public sealed record FgsSetupTaxPatchDto(
    string? TaxCode,
    string? Name,
    bool? IsExternalSystemRecord,
    string? ExternalSystemId,
    string? SyncToken,
    bool? ShowTaxDetail,
    string? Description,
    bool? IsActive);

public sealed record FgsSetupTaxListFilters(
    string? TaxCode = null,
    string? Name = null);
