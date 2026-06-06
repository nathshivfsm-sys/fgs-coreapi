namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Inventory warehouse, truck, trailer, job site, consignment, or vendor storage location.
/// </summary>
public class FgsWarehouse : FgsTenantCompanySetupEntityBase<long>
{
    public string WarehouseCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string WarehouseType { get; set; } = null!;

    public Guid? AddressId { get; set; }

    public string? Description { get; set; }

    public bool IsDefault { get; set; }
}
