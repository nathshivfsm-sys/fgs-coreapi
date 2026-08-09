using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;

namespace Fgs.Inventory.Infrastructure.InventoryItemTypes;

internal sealed class FgsInventoryItemTypeSummaryRow
{
    public long Id { get; set; }
    public string ItemTypeCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool TracksQuantity { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public FgsInventoryItemTypeSummaryDto ToDto() =>
        new(Id, ItemTypeCode, Name, Description, TracksQuantity, DisplayOrder, IsSystem, IsActive);
}

internal sealed class FgsInventoryItemTypeDetailRow
{
    public long Id { get; set; }
    public string ItemTypeCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool TracksQuantity { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public FgsInventoryItemTypeDetailDto ToDto() =>
        new(Id, ItemTypeCode, Name, Description, TracksQuantity, DisplayOrder, IsSystem, IsActive);
}

internal sealed class FgsInventoryItemTypeLookupRow
{
    public long Id { get; set; }
    public string ItemTypeCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsInventoryItemTypeLookupDto ToDto() => new(Id, ItemTypeCode, Name);
}
