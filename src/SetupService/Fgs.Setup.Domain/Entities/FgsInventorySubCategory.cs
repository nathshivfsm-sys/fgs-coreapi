namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped inventory sub-category within a parent category.
/// </summary>
public class FgsInventorySubCategory : FgsTenantCompanySetupEntityBase<long>
{
    public long InventoryCategoryId { get; set; }

    public string SubCategoryCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsSystem { get; set; }
}
