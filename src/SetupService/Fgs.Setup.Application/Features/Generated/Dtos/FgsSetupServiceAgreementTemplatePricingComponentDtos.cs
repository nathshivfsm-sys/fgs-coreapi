namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupServiceAgreementTemplatePricingComponent</summary>
public sealed record FgsSetupServiceAgreementTemplatePricingComponentSummaryDto(
    /// <summary>Service agreement template that includes this pricing component snapshot.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>ServiceAgreementTemplateId</summary>
    long ServiceAgreementTemplateId,
    /// <summary>PricingComponentCode</summary>
    string? PricingComponentCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Amount</summary>
    decimal Amount,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn)
;

public sealed record FgsSetupServiceAgreementTemplatePricingComponentDetailDto(
    /// <summary>Service agreement template that includes this pricing component snapshot.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>ServiceAgreementTemplateId</summary>
    long ServiceAgreementTemplateId,
    /// <summary>PricingComponentCode</summary>
    string? PricingComponentCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Amount</summary>
    decimal Amount,
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

public sealed record FgsSetupServiceAgreementTemplatePricingComponentCreateDto(
    /// <summary>ServiceAgreementTemplateId</summary>
    long ServiceAgreementTemplateId,
    /// <summary>PricingComponentCode</summary>
    string? PricingComponentCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Amount</summary>
    decimal Amount,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder)
;

public sealed record FgsSetupServiceAgreementTemplatePricingComponentUpdateDto(
    /// <summary>ServiceAgreementTemplateId</summary>
    long ServiceAgreementTemplateId,
    /// <summary>PricingComponentCode</summary>
    string? PricingComponentCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Amount</summary>
    decimal Amount,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder)
;

public sealed record FgsSetupServiceAgreementTemplatePricingComponentPatchDto(
    /// <summary>ServiceAgreementTemplateId</summary>
    long? ServiceAgreementTemplateId,
    /// <summary>PricingComponentCode</summary>
    string? PricingComponentCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Amount</summary>
    decimal? Amount,
    /// <summary>DisplayOrder</summary>
    short? DisplayOrder)
;

