using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;

namespace Fgs.Inventory.Infrastructure.TruckStockTemplates;

internal sealed class FgsTruckStockTemplateSummaryRow
{
    public long Id { get; set; }
    public string TemplateCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public FgsTruckStockTemplateSummaryDto ToDto() =>
        new(Id, TemplateCode, Name, Description, IsActive);
}

internal sealed class FgsTruckStockTemplateDetailRow
{
    public long Id { get; set; }
    public string TemplateCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public FgsTruckStockTemplateDetailDto ToDto(IReadOnlyList<FgsTruckStockTemplateItemDetailDto> items) =>
        new(Id, TemplateCode, Name, Description, IsActive, items);
}

internal sealed class FgsTruckStockTemplateItemRow
{
    public long Id { get; set; }
    public long InventoryItemId { get; set; }
    public decimal TargetQuantity { get; set; }
    public decimal MinimumQuantity { get; set; }
    public int DisplayOrder { get; set; }

    public FgsTruckStockTemplateItemDetailDto ToDto() =>
        new(Id, InventoryItemId, TargetQuantity, MinimumQuantity, DisplayOrder);
}

internal sealed class FgsTruckStockTemplateLookupRow
{
    public long Id { get; set; }
    public string TemplateCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsTruckStockTemplateLookupDto ToDto() => new(Id, TemplateCode, Name);
}
