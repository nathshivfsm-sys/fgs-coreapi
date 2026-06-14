namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupTaxDetail</summary>
public sealed record FgsSetupTaxDetailSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>FgsSetupTaxId</summary>
    long FgsSetupTaxId,
    /// <summary>FgsSetupTaxAuthorityId</summary>
    long FgsSetupTaxAuthorityId,
    /// <summary>EffectiveFromDate</summary>
    DateOnly EffectiveFromDate,
    /// <summary>EffectiveToDate</summary>
    DateOnly? EffectiveToDate,
    /// <summary>TaxPercent</summary>
    decimal TaxPercent,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupTaxDetailDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>FgsSetupTaxId</summary>
    long FgsSetupTaxId,
    /// <summary>FgsSetupTaxAuthorityId</summary>
    long FgsSetupTaxAuthorityId,
    /// <summary>EffectiveFromDate</summary>
    DateOnly EffectiveFromDate,
    /// <summary>EffectiveToDate</summary>
    DateOnly? EffectiveToDate,
    /// <summary>TaxPercent</summary>
    decimal TaxPercent,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>CreatedBy</summary>
    string? CreatedBy,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>UpdatedBy</summary>
    string? UpdatedBy,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupTaxDetailCreateDto(
    /// <summary>FgsSetupTaxId</summary>
    long FgsSetupTaxId,
    /// <summary>FgsSetupTaxAuthorityId</summary>
    long FgsSetupTaxAuthorityId,
    /// <summary>EffectiveFromDate</summary>
    DateOnly EffectiveFromDate,
    /// <summary>EffectiveToDate</summary>
    DateOnly? EffectiveToDate,
    /// <summary>TaxPercent</summary>
    decimal TaxPercent,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord)
;

public sealed record FgsSetupTaxDetailUpdateDto(
    /// <summary>FgsSetupTaxId</summary>
    long FgsSetupTaxId,
    /// <summary>FgsSetupTaxAuthorityId</summary>
    long FgsSetupTaxAuthorityId,
    /// <summary>EffectiveFromDate</summary>
    DateOnly EffectiveFromDate,
    /// <summary>EffectiveToDate</summary>
    DateOnly? EffectiveToDate,
    /// <summary>TaxPercent</summary>
    decimal TaxPercent,
    /// <summary>IsExternalSystemRecord</summary>
    bool IsExternalSystemRecord)
;

public sealed record FgsSetupTaxDetailPatchDto(
    /// <summary>FgsSetupTaxId</summary>
    long? FgsSetupTaxId,
    /// <summary>FgsSetupTaxAuthorityId</summary>
    long? FgsSetupTaxAuthorityId,
    /// <summary>EffectiveFromDate</summary>
    DateOnly? EffectiveFromDate,
    /// <summary>EffectiveToDate</summary>
    DateOnly? EffectiveToDate,
    /// <summary>TaxPercent</summary>
    decimal? TaxPercent,
    /// <summary>IsExternalSystemRecord</summary>
    bool? IsExternalSystemRecord)
;

