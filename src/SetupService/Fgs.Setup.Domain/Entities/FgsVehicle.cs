namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Company-owned or leased vehicle used for field service operations.
/// </summary>
public class FgsVehicle : FgsTenantCompanySetupEntityBase<long>
{
    public long WarehouseId { get; set; }

    public string OwnershipType { get; set; } = VehicleOwnershipTypes.Owned;

    public string? OwnershipCompany { get; set; }

    public short? Year { get; set; }

    public string? Make { get; set; }

    public string? Model { get; set; }

    public string? Color { get; set; }

    public string VIN { get; set; } = null!;

    public string? LicensePlate { get; set; }

    public string? LicensePlateState { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public decimal? PurchasePrice { get; set; }

    public string? PurchasedFrom { get; set; }

    public bool? IsPurchasedNew { get; set; }

    public string? Notes { get; set; }
}
