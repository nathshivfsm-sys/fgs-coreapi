namespace Fgs.Inventory.Domain.Entities;

/// <summary>
/// Allowed <see cref="FgsInventoryLocation.InventoryLocationType"/> values.
/// </summary>
public static class InventoryLocationTypes
{
    public const string Warehouse = "WAREHOUSE";

    public const string Truck = "TRUCK";

    public const string Trailer = "TRAILER";

    public const string JobSite = "JOBSITE";

    public const string Consignment = "CONSIGNMENT";

    public const string Vendor = "VENDOR";
}
