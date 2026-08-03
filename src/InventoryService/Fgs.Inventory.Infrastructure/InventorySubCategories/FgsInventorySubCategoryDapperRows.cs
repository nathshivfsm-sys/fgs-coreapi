using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;

namespace Fgs.Inventory.Infrastructure.InventorySubCategories;

internal sealed class FgsInventorySubCategorySummaryRow
{
    public long Id { get; set; }
    public long InventoryCategoryId { get; set; }
    public string SubCategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TextColor { get; set; }
    public string? BackgroundColor { get; set; }
    public long? DisplayIconFileId { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public FgsInventorySubCategorySummaryDto ToDto() =>
        new(Id, InventoryCategoryId, SubCategoryCode, Name, Description, TextColor, BackgroundColor, DisplayIconFileId, DisplayOrder, IsSystem, IsActive);
}

internal sealed class FgsInventorySubCategoryDetailRow
{
    public long Id { get; set; }
    public long InventoryCategoryId { get; set; }
    public string SubCategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TextColor { get; set; }
    public string? BackgroundColor { get; set; }
    public long? DisplayIconFileId { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }

    public FgsInventorySubCategoryDetailDto ToDto() =>
        new(Id, InventoryCategoryId, SubCategoryCode, Name, Description, TextColor, BackgroundColor, DisplayIconFileId, DisplayOrder, IsSystem, IsActive);
}

internal sealed class FgsInventorySubCategoryLookupRow
{
    public long Id { get; set; }
    public string SubCategoryCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsInventorySubCategoryLookupDto ToDto() => new(Id, SubCategoryCode, Name);
}
