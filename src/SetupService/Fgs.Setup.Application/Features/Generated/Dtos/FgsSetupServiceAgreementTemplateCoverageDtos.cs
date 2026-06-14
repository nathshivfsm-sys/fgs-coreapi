namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupServiceAgreementTemplateCoverage</summary>
public sealed record FgsSetupServiceAgreementTemplateCoverageSummaryDto(
    /// <summary>Service agreement template that this coverage item belongs to.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>ServiceAgreementTemplateId</summary>
    long ServiceAgreementTemplateId,
    /// <summary>INCLUDE or EXCLUDE.</summary>
    string? CoverageTypeCode,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn)
;

public sealed record FgsSetupServiceAgreementTemplateCoverageDetailDto(
    /// <summary>Service agreement template that this coverage item belongs to.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>ServiceAgreementTemplateId</summary>
    long ServiceAgreementTemplateId,
    /// <summary>INCLUDE or EXCLUDE.</summary>
    string? CoverageTypeCode,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>CreatedBy</summary>
    string? CreatedBy,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>UpdatedBy</summary>
    string? UpdatedBy)
;

public sealed record FgsSetupServiceAgreementTemplateCoverageCreateDto(
    /// <summary>ServiceAgreementTemplateId</summary>
    long ServiceAgreementTemplateId,
    /// <summary>INCLUDE or EXCLUDE.</summary>
    string? CoverageTypeCode,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder)
;

public sealed record FgsSetupServiceAgreementTemplateCoverageUpdateDto(
    /// <summary>ServiceAgreementTemplateId</summary>
    long ServiceAgreementTemplateId,
    /// <summary>INCLUDE or EXCLUDE.</summary>
    string? CoverageTypeCode,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder)
;

public sealed record FgsSetupServiceAgreementTemplateCoveragePatchDto(
    /// <summary>ServiceAgreementTemplateId</summary>
    long? ServiceAgreementTemplateId,
    /// <summary>INCLUDE or EXCLUDE.</summary>
    string? CoverageTypeCode,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short? DisplayOrder)
;

