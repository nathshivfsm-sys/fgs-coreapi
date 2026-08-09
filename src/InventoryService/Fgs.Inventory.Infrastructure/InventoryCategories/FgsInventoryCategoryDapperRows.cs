using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;

namespace Fgs.Inventory.Infrastructure.InventoryCategories;

internal sealed class FgsInventoryCategorySummaryRow
{
    public long Id { get; set; }
    public string CategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TextColor { get; set; }
    public string? BackgroundColor { get; set; }
    public long? DisplayIconFileId { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public FgsInventoryCategorySummaryDto ToDto() =>
        new(Id, CategoryCode, Name, Description, TextColor, BackgroundColor, DisplayIconFileId, DisplayOrder, IsSystem, IsActive);
}

internal sealed class FgsInventoryCategoryDetailRow
{
    public long Id { get; set; }
    public string CategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TextColor { get; set; }
    public string? BackgroundColor { get; set; }
    public long? DisplayIconFileId { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public FgsInventoryCategoryDetailDto ToDto() =>
        new(Id, CategoryCode, Name, Description, TextColor, BackgroundColor, DisplayIconFileId, DisplayOrder, IsSystem, IsActive);
}

internal sealed class FgsInventoryCategoryLookupRow
{
    public long Id { get; set; }
    public string CategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsInventoryCategoryLookupDto ToDto() => new(Id, CategoryCode, Name);
}
