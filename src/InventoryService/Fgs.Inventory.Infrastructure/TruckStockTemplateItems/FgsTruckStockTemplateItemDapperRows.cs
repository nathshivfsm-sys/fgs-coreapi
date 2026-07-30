using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;

namespace Fgs.Inventory.Infrastructure.TruckStockTemplateItems;

internal sealed class FgsTruckStockTemplateItemSummaryRow
{
    public long Id { get; set; }
    public long TruckStockTemplateId { get; set; }
    public long InventoryItemId { get; set; }
    public decimal TargetQuantity { get; set; }
    public decimal MinimumQuantity { get; set; }
    public int DisplayOrder { get; set; }

    public FgsTruckStockTemplateItemSummaryDto ToDto() =>
        new(Id, TruckStockTemplateId, InventoryItemId, TargetQuantity, MinimumQuantity, DisplayOrder);
}

internal sealed class FgsTruckStockTemplateItemDetailRow
{
    public long Id { get; set; }
    public long TruckStockTemplateId { get; set; }
    public long InventoryItemId { get; set; }
    public decimal TargetQuantity { get; set; }
    public decimal MinimumQuantity { get; set; }
    public int DisplayOrder { get; set; }

    public FgsTruckStockTemplateItemDetailDto ToDto() =>
        new(Id, TruckStockTemplateId, InventoryItemId, TargetQuantity, MinimumQuantity, DisplayOrder);
}
