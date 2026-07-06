using Fgs.Kernel.Entities;

namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Inventory location including warehouses, trucks, trailers, job sites, consignment, and vendor locations.
/// </summary>
public class FgsInventoryLocation : FgsTenantCompanySetupEntityBase<long>
{
    public string InventoryLocationCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string InventoryLocationType { get; set; } = null!;

    public long? ParentInventoryLocationId { get; set; }

    public string? Description { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? StateProvince { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? ContactName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? TextColor { get; set; }

    public string? BackgroundColor { get; set; }

    /// <summary>References file.FgsFile; scalar only — no cross-schema FK.</summary>
    public long? DisplayIconFileId { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsDefault { get; set; }
}
