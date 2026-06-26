namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global inventory sub-category within a parent category.
/// </summary>
public class GloInventorySubCategory
{
    public int Id { get; set; }

    public int InventoryCategoryId { get; set; }

    public string SubCategoryCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }
}
