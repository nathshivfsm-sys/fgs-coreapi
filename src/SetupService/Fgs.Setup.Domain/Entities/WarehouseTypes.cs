namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Allowed <see cref="FgsWarehouse.WarehouseType"/> values (see CK_FgsWarehouse_WarehouseType).
/// </summary>
public static class WarehouseTypes
{
    public const string Warehouse = "Warehouse";

    public const string Truck = "Truck";

    public const string Trailer = "Trailer";

    public const string JobSite = "JobSite";

    public const string Consignment = "Consignment";

    public const string Vendor = "Vendor";
}
