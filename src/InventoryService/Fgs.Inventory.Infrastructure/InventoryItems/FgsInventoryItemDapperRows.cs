using Fgs.Inventory.Application.Features.InventoryItems.Dtos;

namespace Fgs.Inventory.Infrastructure.InventoryItems;

internal sealed class FgsInventoryItemSummaryRow
{
    public long Id { get; set; }
    public string ItemCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public long InventoryItemTypeId { get; set; }
    public long? InventoryCategoryId { get; set; }
    public long? InventorySubCategoryId { get; set; }
    public bool TracksInventory { get; set; }
    public bool IsSerialized { get; set; }
    public decimal UnitCost { get; set; }
    public decimal SalesPrice { get; set; }
    public bool IsActive { get; set; }

    public FgsInventoryItemSummaryDto ToDto() =>
        new(
            Id,
            ItemCode,
            Name,
            InventoryItemTypeId,
            InventoryCategoryId,
            InventorySubCategoryId,
            TracksInventory,
            IsSerialized,
            UnitCost,
            SalesPrice,
            IsActive);
}

internal sealed class FgsInventoryItemDetailRow
{
    public long Id { get; set; }
    public long InventoryItemTypeId { get; set; }
    public long? InventoryCategoryId { get; set; }
    public long? InventorySubCategoryId { get; set; }
    public string ItemCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? PurchaseDescription { get; set; }
    public string? SalesDescription { get; set; }
    public string? ManufacturerPartNumber { get; set; }
    public string? ManufacturerName { get; set; }
    public string? Sku { get; set; }
    public string? UPCCode { get; set; }
    public string? UnitOfMeasure { get; set; }
    public bool TracksInventory { get; set; }
    public bool IsSerialized { get; set; }
    public decimal UnitCost { get; set; }
    public decimal StandardUnitCost { get; set; }
    public decimal SalesPrice { get; set; }
    public bool IsActive { get; set; }

    public FgsInventoryItemDetailDto ToDto(
        IReadOnlyList<FgsInventoryItemAlternateDetailDto> alternates,
        IReadOnlyList<FgsInventoryItemDependencyDetailDto> dependencies) =>
        new(
            Id,
            InventoryItemTypeId,
            InventoryCategoryId,
            InventorySubCategoryId,
            ItemCode,
            Name,
            Description,
            PurchaseDescription,
            SalesDescription,
            ManufacturerPartNumber,
            ManufacturerName,
            Sku,
            UPCCode,
            UnitOfMeasure,
            TracksInventory,
            IsSerialized,
            UnitCost,
            StandardUnitCost,
            SalesPrice,
            IsActive,
            alternates,
            dependencies);
}

internal sealed class FgsInventoryItemAlternateRow
{
    public long Id { get; set; }
    public long AlternateInventoryItemId { get; set; }
    public short PriorityOrder { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }

    public FgsInventoryItemAlternateDetailDto ToDto() =>
        new(Id, AlternateInventoryItemId, PriorityOrder, Notes, IsActive);
}

internal sealed class FgsInventoryItemDependencyRow
{
    public long Id { get; set; }
    public long DependentInventoryItemId { get; set; }
    public decimal Quantity { get; set; }
    public bool IsRequired { get; set; }
    public string? Notes { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    public FgsInventoryItemDependencyDetailDto ToDto() =>
        new(Id, DependentInventoryItemId, Quantity, IsRequired, Notes, DisplayOrder, IsActive);
}

internal sealed class FgsInventoryItemLookupRow
{
    public long Id { get; set; }
    public string ItemCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsInventoryItemLookupDto ToDto() => new(Id, ItemCode, Name);
}
