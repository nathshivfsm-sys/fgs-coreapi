namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>Stores tenant/company specific billing categories used for invoicing, service billing, maintenance plans, and other billing operations. Seeded initially from GloBillingCategory but fully managed by each tenant/company independently.</summary>
public sealed record FgsBillingCategorySummaryDto(
    /// <summary>Primary key identity of the billing category record.</summary>
    long Id,
    /// <summary>Tenant identifier owning this billing category.</summary>
    long TenantId,
    /// <summary>Company identifier within the tenant owning this billing category.</summary>
    long CompanyId,
    /// <summary>Short billing category code such as IN, PM, SR, or other tenant-defined values.</summary>
    string? BillingCategoryType,
    /// <summary>Display name of the billing category shown throughout the application.</summary>
    string? BillingCategoryName,
    /// <summary>Optional internal description or notes for the billing category.</summary>
    string? Description,
    /// <summary>Controls sorting/display order of billing categories in dropdowns and setup screens.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the billing category was system seeded or manually created by the tenant/company.</summary>
    bool IsSystemDefined,
    /// <summary>Date and time the billing category record was created.</summary>
    bool ShowToFieldTech,
    /// <summary>AllowToPick</summary>
    bool AllowToPick,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>Date and time the billing category record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>Indicates whether the billing category is active and available for use.</summary>
    bool IsActive)
;

public sealed record FgsBillingCategoryDetailDto(
    /// <summary>Primary key identity of the billing category record.</summary>
    long Id,
    /// <summary>Tenant identifier owning this billing category.</summary>
    long TenantId,
    /// <summary>Company identifier within the tenant owning this billing category.</summary>
    long CompanyId,
    /// <summary>Short billing category code such as IN, PM, SR, or other tenant-defined values.</summary>
    string? BillingCategoryType,
    /// <summary>Display name of the billing category shown throughout the application.</summary>
    string? BillingCategoryName,
    /// <summary>Optional internal description or notes for the billing category.</summary>
    string? Description,
    /// <summary>Controls sorting/display order of billing categories in dropdowns and setup screens.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the billing category was system seeded or manually created by the tenant/company.</summary>
    bool IsSystemDefined,
    /// <summary>Date and time the billing category record was created.</summary>
    bool ShowToFieldTech,
    /// <summary>AllowToPick</summary>
    bool AllowToPick,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>User identifier that created the billing category record.</summary>
    string? CreatedBy,
    /// <summary>Date and time the billing category record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>User identifier that last updated the billing category record.</summary>
    string? UpdatedBy,
    /// <summary>Indicates whether the billing category is active and available for use.</summary>
    bool IsActive)
;

public sealed record FgsBillingCategoryCreateDto(
    /// <summary>Short billing category code such as IN, PM, SR, or other tenant-defined values.</summary>
    string? BillingCategoryType,
    /// <summary>Display name of the billing category shown throughout the application.</summary>
    string? BillingCategoryName,
    /// <summary>Optional internal description or notes for the billing category.</summary>
    string? Description,
    /// <summary>Controls sorting/display order of billing categories in dropdowns and setup screens.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the billing category was system seeded or manually created by the tenant/company.</summary>
    bool IsSystemDefined,
    /// <summary>Date and time the billing category record was created.</summary>
    bool ShowToFieldTech,
    /// <summary>AllowToPick</summary>
    bool AllowToPick)
;

public sealed record FgsBillingCategoryUpdateDto(
    /// <summary>Short billing category code such as IN, PM, SR, or other tenant-defined values.</summary>
    string? BillingCategoryType,
    /// <summary>Display name of the billing category shown throughout the application.</summary>
    string? BillingCategoryName,
    /// <summary>Optional internal description or notes for the billing category.</summary>
    string? Description,
    /// <summary>Controls sorting/display order of billing categories in dropdowns and setup screens.</summary>
    short DisplayOrder,
    /// <summary>Indicates whether the billing category was system seeded or manually created by the tenant/company.</summary>
    bool IsSystemDefined,
    /// <summary>Date and time the billing category record was created.</summary>
    bool ShowToFieldTech,
    /// <summary>AllowToPick</summary>
    bool AllowToPick)
;

public sealed record FgsBillingCategoryPatchDto(
    /// <summary>Short billing category code such as IN, PM, SR, or other tenant-defined values.</summary>
    string? BillingCategoryType,
    /// <summary>Display name of the billing category shown throughout the application.</summary>
    string? BillingCategoryName,
    /// <summary>Optional internal description or notes for the billing category.</summary>
    string? Description,
    /// <summary>Controls sorting/display order of billing categories in dropdowns and setup screens.</summary>
    short? DisplayOrder,
    /// <summary>Indicates whether the billing category was system seeded or manually created by the tenant/company.</summary>
    bool? IsSystemDefined,
    /// <summary>Date and time the billing category record was created.</summary>
    bool? ShowToFieldTech,
    /// <summary>AllowToPick</summary>
    bool? AllowToPick)
;

