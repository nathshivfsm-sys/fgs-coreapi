namespace Fgs.Setup.Application.Features.Vehicles.Dtos;

public sealed record FgsVehicleSummaryDto(
    long Id,
    long WarehouseId,
    string OwnershipType,
    string? OwnershipCompany,
    short? Year,
    string? Make,
    string? Model,
    string? Color,
    string VIN,
    string? LicensePlate,
    string? LicensePlateState,
    DateOnly? PurchaseDate,
    decimal? PurchasePrice,
    string? PurchasedFrom,
    bool? IsPurchasedNew,
    string? Notes,
    bool IsActive);

public sealed record FgsVehicleDetailDto(
    long Id,
    long WarehouseId,
    string OwnershipType,
    string? OwnershipCompany,
    short? Year,
    string? Make,
    string? Model,
    string? Color,
    string VIN,
    string? LicensePlate,
    string? LicensePlateState,
    DateOnly? PurchaseDate,
    decimal? PurchasePrice,
    string? PurchasedFrom,
    bool? IsPurchasedNew,
    string? Notes,
    bool IsActive);

public sealed record FgsVehicleLookupDto(
    long Id,
    string? Make,
    string? Model,
    string VIN);

public sealed record FgsVehicleCreateDto(
    long WarehouseId,
    string OwnershipType,
    string? OwnershipCompany,
    short? Year,
    string? Make,
    string? Model,
    string? Color,
    string VIN,
    string? LicensePlate,
    string? LicensePlateState,
    DateOnly? PurchaseDate,
    decimal? PurchasePrice,
    string? PurchasedFrom,
    bool? IsPurchasedNew,
    string? Notes);

public sealed record FgsVehicleUpdateDto(
    long WarehouseId,
    string OwnershipType,
    string? OwnershipCompany,
    short? Year,
    string? Make,
    string? Model,
    string? Color,
    string VIN,
    string? LicensePlate,
    string? LicensePlateState,
    DateOnly? PurchaseDate,
    decimal? PurchasePrice,
    string? PurchasedFrom,
    bool? IsPurchasedNew,
    string? Notes);

public sealed record FgsVehiclePatchDto(
    long? WarehouseId,
    string? OwnershipType,
    string? OwnershipCompany,
    short? Year,
    string? Make,
    string? Model,
    string? Color,
    string? VIN,
    string? LicensePlate,
    string? LicensePlateState,
    DateOnly? PurchaseDate,
    decimal? PurchasePrice,
    string? PurchasedFrom,
    bool? IsPurchasedNew,
    string? Notes,
    bool? IsActive);

public sealed record FgsVehicleListFilters(
    string? VIN = null);
