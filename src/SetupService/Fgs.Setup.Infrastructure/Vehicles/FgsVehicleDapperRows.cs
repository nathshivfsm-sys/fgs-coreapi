using Fgs.Setup.Application.Features.Vehicles.Dtos;

namespace Fgs.Setup.Infrastructure.Vehicles;

internal sealed class FgsVehicleSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long WarehouseId { get; set; }
    public string OwnershipType { get; set; }
    public string? OwnershipCompany { get; set; }
    public short? Year { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public string VIN { get; set; }
    public string? LicensePlate { get; set; }
    public string? LicensePlateState { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
    public string? PurchasedFrom { get; set; }
    public bool? IsPurchasedNew { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsVehicleSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            WarehouseId,
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
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsVehicleDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long WarehouseId { get; set; }
    public string OwnershipType { get; set; }
    public string? OwnershipCompany { get; set; }
    public short? Year { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public string VIN { get; set; }
    public string? LicensePlate { get; set; }
    public string? LicensePlateState { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
    public string? PurchasedFrom { get; set; }
    public bool? IsPurchasedNew { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsVehicleDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
            WarehouseId,
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
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
}

internal sealed class FgsVehicleLookupRow
{
    public long Id { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string VIN { get; set; }

    public FgsVehicleLookupDto ToDto() => new(Id,
            Make,
            Model,
            VIN);
}
