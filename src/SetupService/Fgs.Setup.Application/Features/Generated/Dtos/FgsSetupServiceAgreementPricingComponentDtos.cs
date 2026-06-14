namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupServiceAgreementPricingComponent</summary>
public sealed record FgsSetupServiceAgreementPricingComponentSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>PricingComponentCode</summary>
    string? PricingComponentCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>PricingComponentTypeCode</summary>
    string? PricingComponentTypeCode,
    /// <summary>Amount</summary>
    decimal Amount,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupServiceAgreementPricingComponentDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>PricingComponentCode</summary>
    string? PricingComponentCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>PricingComponentTypeCode</summary>
    string? PricingComponentTypeCode,
    /// <summary>Amount</summary>
    decimal Amount,
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
    string? UpdatedBy,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupServiceAgreementPricingComponentCreateDto(
    /// <summary>PricingComponentCode</summary>
    string? PricingComponentCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>PricingComponentTypeCode</summary>
    string? PricingComponentTypeCode,
    /// <summary>Amount</summary>
    decimal Amount,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder)
;

public sealed record FgsSetupServiceAgreementPricingComponentUpdateDto(
    /// <summary>PricingComponentCode</summary>
    string? PricingComponentCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>PricingComponentTypeCode</summary>
    string? PricingComponentTypeCode,
    /// <summary>Amount</summary>
    decimal Amount,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder)
;

public sealed record FgsSetupServiceAgreementPricingComponentPatchDto(
    /// <summary>PricingComponentCode</summary>
    string? PricingComponentCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>PricingComponentTypeCode</summary>
    string? PricingComponentTypeCode,
    /// <summary>Amount</summary>
    decimal? Amount,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>DisplayOrder</summary>
    short? DisplayOrder)
;

