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
    bool IsActive);

public sealed record FgsTruckStockTemplateLookupDto(
    long Id,
    string TemplateCode,
    string Name);

public sealed record FgsTruckStockTemplateCreateDto(
    string TemplateCode,
    string Name,
    string? Description);

public sealed record FgsTruckStockTemplateUpdateDto(
    string TemplateCode,
    string Name,
    string? Description);

public sealed record FgsTruckStockTemplatePatchDto(
    string? TemplateCode,
    string? Name,
    string? Description,
    bool? IsActive);

public sealed record FgsTruckStockTemplateListFilters(
    string? TemplateCode = null,
    string? Name = null);
