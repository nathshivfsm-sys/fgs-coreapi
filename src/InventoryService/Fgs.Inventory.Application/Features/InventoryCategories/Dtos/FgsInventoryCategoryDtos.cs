namespace Fgs.Inventory.Application.Features.InventoryCategories.Dtos;

public sealed record FgsInventoryCategorySummaryDto(
    long Id,
    string CategoryCode,
    string Name,
    string? Description,
    string? TextColor,
    string? BackgroundColor,
    long? DisplayIconFileId,
    short DisplayOrder,
    bool IsSystem,
    bool IsActive);

public sealed record FgsInventoryCategoryDetailDto(
    long Id,
    string CategoryCode,
    string Name,
    string? Description,
    string? TextColor,
    string? BackgroundColor,
    long? DisplayIconFileId,
    short DisplayOrder,
    bool IsSystem,
    bool IsActive);

public sealed record FgsInventoryCategoryLookupDto(
    long Id,
    string CategoryCode,
    string Name);

public sealed record FgsInventoryCategoryCreateDto(
    string CategoryCode,
    string Name,
    string? Description,
    string? TextColor,
    string? BackgroundColor,
    long? DisplayIconFileId,
    short DisplayOrder = 1);

public sealed record FgsInventoryCategoryUpdateDto(
    string CategoryCode,
    string Name,
    string? Description,
    string? TextColor,
    string? BackgroundColor,
    long? DisplayIconFileId,
    short DisplayOrder);

public sealed record FgsInventoryCategoryPatchDto(
    string? CategoryCode,
    string? Name,
    string? Description,
    string? TextColor,
    string? BackgroundColor,
    long? DisplayIconFileId,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsInventoryCategoryListFilters(
    string? CategoryCode = null,
    string? Name = null);
