namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsSetupServiceAgreementTemplate</summary>
public sealed record FgsSetupServiceAgreementTemplateSummaryDto(
    /// <summary>Discount given to service agreement customers on additional repairs.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>TemplateCode</summary>
    string? TemplateCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>BillingFrequencyMonths</summary>
    short BillingFrequencyMonths,
    /// <summary>MaintenanceFrequencyMonths</summary>
    short MaintenanceFrequencyMonths,
    /// <summary>RepairDiscountPercent</summary>
    decimal RepairDiscountPercent,
    /// <summary>IsAutoRenew</summary>
    bool IsAutoRenew,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsSetupServiceAgreementTemplateDetailDto(
    /// <summary>Discount given to service agreement customers on additional repairs.</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>TemplateCode</summary>
    string? TemplateCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>BillingFrequencyMonths</summary>
    short BillingFrequencyMonths,
    /// <summary>MaintenanceFrequencyMonths</summary>
    short MaintenanceFrequencyMonths,
    /// <summary>RepairDiscountPercent</summary>
    decimal RepairDiscountPercent,
    /// <summary>IsAutoRenew</summary>
    bool IsAutoRenew,
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

public sealed record FgsSetupServiceAgreementTemplateCreateDto(
    /// <summary>TemplateCode</summary>
    string? TemplateCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>BillingFrequencyMonths</summary>
    short BillingFrequencyMonths,
    /// <summary>MaintenanceFrequencyMonths</summary>
    short MaintenanceFrequencyMonths,
    /// <summary>RepairDiscountPercent</summary>
    decimal RepairDiscountPercent,
    /// <summary>IsAutoRenew</summary>
    bool IsAutoRenew,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder)
;

public sealed record FgsSetupServiceAgreementTemplateUpdateDto(
    /// <summary>TemplateCode</summary>
    string? TemplateCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>BillingFrequencyMonths</summary>
    short BillingFrequencyMonths,
    /// <summary>MaintenanceFrequencyMonths</summary>
    short MaintenanceFrequencyMonths,
    /// <summary>RepairDiscountPercent</summary>
    decimal RepairDiscountPercent,
    /// <summary>IsAutoRenew</summary>
    bool IsAutoRenew,
    /// <summary>DisplayOrder</summary>
    short DisplayOrder)
;

public sealed record FgsSetupServiceAgreementTemplatePatchDto(
    /// <summary>TemplateCode</summary>
    string? TemplateCode,
    /// <summary>Name</summary>
    string? Name,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>BillingFrequencyMonths</summary>
    short? BillingFrequencyMonths,
    /// <summary>MaintenanceFrequencyMonths</summary>
    short? MaintenanceFrequencyMonths,
    /// <summary>RepairDiscountPercent</summary>
    decimal? RepairDiscountPercent,
    /// <summary>IsAutoRenew</summary>
    bool? IsAutoRenew,
    /// <summary>DisplayOrder</summary>
    short? DisplayOrder)
;

