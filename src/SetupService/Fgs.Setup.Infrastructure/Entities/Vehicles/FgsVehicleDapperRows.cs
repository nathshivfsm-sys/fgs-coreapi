using Fgs.Setup.Application.Features.Vehicles.Dtos;

namespace Fgs.Setup.Infrastructure.Entities.Vehicles;

internal sealed class FgsVehicleSummaryRow
{
    public long Id { get; set; }
    public long InventoryLocationId { get; set; }
    public string OwnershipType { get; set; } = null!;
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
    public bool IsActive { get; set; }

    public FgsVehicleSummaryDto ToDto() =>
        new(
            Id,
            InventoryLocationId,
            OwnershipType,
            OwnershipCompany,
            Year,
            Make,
            Model,
            Color,
            VIN,
            LicensePlate,
            LicensePlateState,
            PurchaseDate,
            PurchasePrice,
            PurchasedFrom,
            IsPurchasedNew,
            Notes,
            IsActive);
}

internal sealed class FgsVehicleDetailRow
{
    public long Id { get; set; }
    public long InventoryLocationId { get; set; }
    public string OwnershipType { get; set; } = null!;
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
    public bool IsActive { get; set; }

    public FgsVehicleDetailDto ToDto() =>
        new(
            Id,
            InventoryLocationId,
            OwnershipType,
            OwnershipCompany,
            Year,
            Make,
            Model,
            Color,
            VIN,
            LicensePlate,
            LicensePlateState,
            PurchaseDate,
            PurchasePrice,
            PurchasedFrom,
            IsPurchasedNew,
            Notes,
            IsActive);
}

internal sealed class FgsVehicleLookupRow
{
    public long Id { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string VIN { get; set; } = null!;

    public FgsVehicleLookupDto ToDto() => new(Id,
            Make,
            Model,
            VIN);
}
