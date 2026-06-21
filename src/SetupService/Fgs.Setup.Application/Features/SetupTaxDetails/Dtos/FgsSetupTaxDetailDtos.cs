namespace Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;

public sealed record FgsSetupTaxDetailSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    long FgsSetupTaxId,
    long FgsSetupTaxAuthorityId,
    DateOnly EffectiveFromDate,
    DateOnly? EffectiveToDate,
    decimal TaxPercent,
    bool IsExternalSystemRecord,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSetupTaxDetailDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    long FgsSetupTaxId,
    long FgsSetupTaxAuthorityId,
    DateOnly EffectiveFromDate,
    DateOnly? EffectiveToDate,
    decimal TaxPercent,
    bool IsExternalSystemRecord,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsSetupTaxDetailLookupDto(
    long Id,
    long FgsSetupTaxId,
    long FgsSetupTaxAuthorityId,
    DateOnly EffectiveFromDate,
    decimal TaxPercent);

public sealed record FgsSetupTaxDetailCreateDto(
    long FgsSetupTaxId,
    long FgsSetupTaxAuthorityId,
    DateOnly EffectiveFromDate,
    DateOnly? EffectiveToDate,
    decimal TaxPercent,
    bool IsExternalSystemRecord);

public sealed record FgsSetupTaxDetailUpdateDto(
    long FgsSetupTaxId,
    long FgsSetupTaxAuthorityId,
    DateOnly EffectiveFromDate,
    DateOnly? EffectiveToDate,
    decimal TaxPercent,
    bool IsExternalSystemRecord);

public sealed record FgsSetupTaxDetailPatchDto(
    long? FgsSetupTaxId,
    long? FgsSetupTaxAuthorityId,
    DateOnly? EffectiveFromDate,
    DateOnly? EffectiveToDate,
    decimal? TaxPercent,
    bool? IsExternalSystemRecord,
    bool? IsActive);

public sealed record FgsSetupTaxDetailListFilters();
