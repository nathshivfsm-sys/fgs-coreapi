namespace Fgs.Setup.Application.Features.BillingCategories.Dtos;

public sealed record BillingCategorySummaryDto(
    long Id,
    string BillingCategoryType,
    string BillingCategoryName,
    string? Description,
    short? DisplayOrder,
    bool IsSystemDefined,
    bool ShowToFieldTech,
    bool AllowToPick,
    bool IsActive);

public sealed record BillingCategoryDetailDto(
    long Id,
    string BillingCategoryType,
    string BillingCategoryName,
    string? Description,
    short? DisplayOrder);

public sealed record BillingCategoryLookupDto(
    long Id,
    string BillingCategoryType,
    string BillingCategoryName,
    short? DisplayOrder);

public sealed record BillingCategoryCreateDto(
    string BillingCategoryType,
    string BillingCategoryName,
    string? Description,
    short? DisplayOrder,
    bool IsSystemDefined,
    bool ShowToFieldTech,
    bool AllowToPick);

public sealed record BillingCategoryUpdateDto(
    string BillingCategoryType,
    string BillingCategoryName,
    string? Description,
    short? DisplayOrder,
    bool IsSystemDefined,
    bool ShowToFieldTech,
    bool AllowToPick);

public sealed record BillingCategoryPatchDto(
    string? BillingCategoryType,
    string? BillingCategoryName,
    string? Description,
    short? DisplayOrder,
    bool? IsSystemDefined,
    bool? ShowToFieldTech,
    bool? AllowToPick,
    bool? IsActive);

public sealed record BillingCategoryListFilters(
    string? BillingCategoryType = null,
    string? BillingCategoryName = null,
    bool? ShowToFieldTech = null,
    bool? AllowToPick = null);
