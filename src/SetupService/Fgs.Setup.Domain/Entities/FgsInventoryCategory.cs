namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant- and company-scoped inventory category catalog.
/// </summary>
public class FgsInventoryCategory : FgsTenantCompanySetupEntityBase<long>
{
    public string CategoryCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsSystem { get; set; }
}
