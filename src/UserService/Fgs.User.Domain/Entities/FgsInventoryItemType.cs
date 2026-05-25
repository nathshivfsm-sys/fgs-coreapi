namespace Fgs.User.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped inventory item type catalog.
/// </summary>
public class FgsInventoryItemType : FgsTenantCompanySetupEntityBase<long>
{
    public string ItemTypeCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool TracksQuantity { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsSystem { get; set; }
}
