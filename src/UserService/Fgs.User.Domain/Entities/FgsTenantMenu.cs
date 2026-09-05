namespace Fgs.User.Domain.Entities;

/// <summary>
/// Stores the menu items enabled for a company within a tenant based on the tenant subscription and available platform features.
/// Catalog fields are denormalized from glo.GloMenu at provision/sync time.
/// </summary>
public class FgsTenantMenu : FgsTenantCompanySetupEntityBase<long>
{
    public int MenuId { get; set; }

    public string MenuCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? ParentMenuId { get; set; }

    public string MenuType { get; set; } = null!;

    public string? Route { get; set; }

    public string? Icon { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
