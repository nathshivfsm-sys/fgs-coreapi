namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;

public sealed record FgsTruckStockTemplateSummaryDto(
    long Id,
    string TemplateCode,
    string Name,
    string? Description,
    bool IsActive);

public sealed record FgsTruckStockTemplateDetailDto(
    long Id,
    string TemplateCode,
    string Name,
    string? Description,
    bool IsActive,
    IReadOnlyList<FgsTruckStockTemplateItemDetailDto> Items);

public sealed record FgsTruckStockTemplateLookupDto(
    long Id,
    string TemplateCode,
    string Name);

public sealed record FgsTruckStockTemplateItemDetailDto(
    long Id,
    long InventoryItemId,
    decimal TargetQuantity,
    decimal MinimumQuantity,
    int DisplayOrder);

/// <summary>
/// Nested item on create/update/patch. Omit <see cref="Id"/> (or null) to insert; supply Id to update.
/// Omitted existing items are removed on create/update (and on patch when Items is provided).
/// </summary>
public sealed record FgsTruckStockTemplateItemDto(
    long? Id,
    long InventoryItemId,
    decimal TargetQuantity,
    decimal MinimumQuantity,
    int DisplayOrder = 1);

public sealed record FgsTruckStockTemplateCreateDto(
    string TemplateCode,
    string Name,
    string? Description,
    IReadOnlyList<FgsTruckStockTemplateItemDto>? Items = null);

public sealed record FgsTruckStockTemplateUpdateDto(
    string TemplateCode,
    string Name,
    string? Description,
    IReadOnlyList<FgsTruckStockTemplateItemDto>? Items = null);

public sealed record FgsTruckStockTemplatePatchDto(
    string? TemplateCode,
    string? Name,
    string? Description,
    bool? IsActive,
    IReadOnlyList<FgsTruckStockTemplateItemDto>? Items = null);

public sealed record FgsTruckStockTemplateListFilters(
    string? TemplateCode = null,
    string? Name = null);
