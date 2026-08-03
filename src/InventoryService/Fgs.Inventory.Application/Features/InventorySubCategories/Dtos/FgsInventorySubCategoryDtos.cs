namespace Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;

public sealed record FgsInventorySubCategorySummaryDto(
    long Id,
    long InventoryCategoryId,
    string SubCategoryCode,
    string Name,
    string? Description,
    string? TextColor,
    string? BackgroundColor,
    long? DisplayIconFileId,
    short DisplayOrder,
    bool IsSystem,
    bool IsActive);

public sealed record FgsInventorySubCategoryDetailDto(
    long Id,
    long InventoryCategoryId,
    string SubCategoryCode,
    string Name,
    string? Description,
    string? TextColor,
    string? BackgroundColor,
    long? DisplayIconFileId,
    short DisplayOrder,
    bool IsSystem,
    bool IsActive);

public sealed record FgsInventorySubCategoryLookupDto(
    long Id,
    string SubCategoryCode,
    string Name);

public sealed record FgsInventorySubCategoryCreateDto(
    long InventoryCategoryId,
    string SubCategoryCode,
    string Name,
    string? Description,
    string? TextColor,
    string? BackgroundColor,
    long? DisplayIconFileId,
    short DisplayOrder = 1);

public sealed record FgsInventorySubCategoryUpdateDto(
    long InventoryCategoryId,
    string SubCategoryCode,
    string Name,
    string? Description,
    string? TextColor,
    string? BackgroundColor,
    long? DisplayIconFileId,
    short DisplayOrder);

public sealed record FgsInventorySubCategoryPatchDto(
    long? InventoryCategoryId,
    string? SubCategoryCode,
    string? Name,
    string? Description,
    string? TextColor,
    string? BackgroundColor,
    long? DisplayIconFileId,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsInventorySubCategoryListFilters(
    string? SubCategoryCode = null,
    string? Name = null,
    long? InventoryCategoryId = null);
