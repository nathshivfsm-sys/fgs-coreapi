namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;

public sealed record FgsInventoryItemTypeSummaryDto(
    long Id,
    string ItemTypeCode,
    string Name,
    string? Description,
    bool TracksQuantity,
    short DisplayOrder,
    bool IsSystem,
    bool IsActive);

public sealed record FgsInventoryItemTypeDetailDto(
    long Id,
    string ItemTypeCode,
    string Name,
    string? Description,
    bool TracksQuantity,
    short DisplayOrder,
    bool IsSystem,
    bool IsActive);

public sealed record FgsInventoryItemTypeLookupDto(
    long Id,
    string ItemTypeCode,
    string Name);

public sealed record FgsInventoryItemTypeCreateDto(
    string ItemTypeCode,
    string Name,
    string? Description,
    bool TracksQuantity,
    short DisplayOrder = 1);

public sealed record FgsInventoryItemTypeUpdateDto(
    string ItemTypeCode,
    string Name,
    string? Description,
    bool TracksQuantity,
    short DisplayOrder);

public sealed record FgsInventoryItemTypePatchDto(
    string? ItemTypeCode,
    string? Name,
    string? Description,
    bool? TracksQuantity,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsInventoryItemTypeListFilters(
    string? ItemTypeCode = null,
    string? Name = null);
