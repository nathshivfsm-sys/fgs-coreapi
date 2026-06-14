namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsVehicle</summary>
public sealed record FgsVehicleSummaryDto(
    /// <summary>Primary key.</summary>
    long Id,
    /// <summary>Tenant identifier.</summary>
    long TenantId,
    /// <summary>Company identifier.</summary>
    long CompanyId,
    /// <summary>Associated truck warehouse used as the vehicle inventory location.</summary>
    long WarehouseId,
    /// <summary>Indicates whether the vehicle is owned, leased, or rented.</summary>
    string? OwnershipType,
    /// <summary>Vehicle model year.</summary>
    string? OwnershipCompany,
    /// <summary>Year</summary>
    short? Year,
    /// <summary>Vehicle manufacturer such as Ford, Chevrolet, GMC, Ram, Toyota, etc.</summary>
    string? Make,
    /// <summary>Vehicle model such as F-150, Transit, Silverado, Express, etc.</summary>
    string? Model,
    /// <summary>Vehicle exterior color.</summary>
    string? Color,
    /// <summary>Vehicle Identification Number assigned by the manufacturer.</summary>
    string? VIN,
    /// <summary>Vehicle registration plate number.</summary>
    string? LicensePlate,
    /// <summary>State or province issuing the vehicle registration.</summary>
    string? LicensePlateState,
    /// <summary>Date the vehicle was purchased or acquired.</summary>
    DateOnly? PurchaseDate,
    /// <summary>Amount paid to acquire the vehicle.</summary>
    decimal? PurchasePrice,
    /// <summary>Indicates whether the vehicle was purchased new or used.</summary>
    string? PurchasedFrom,
    /// <summary>IsPurchasedNew</summary>
    bool? IsPurchasedNew,
    /// <summary>Internal notes and remarks regarding the vehicle.</summary>
    string? Notes,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>Indicates whether the vehicle is active and available for service operations.</summary>
    bool IsActive)
;

public sealed record FgsVehicleDetailDto(
    /// <summary>Primary key.</summary>
    long Id,
    /// <summary>Tenant identifier.</summary>
    long TenantId,
    /// <summary>Company identifier.</summary>
    long CompanyId,
    /// <summary>Associated truck warehouse used as the vehicle inventory location.</summary>
    long WarehouseId,
    /// <summary>Indicates whether the vehicle is owned, leased, or rented.</summary>
    string? OwnershipType,
    /// <summary>Vehicle model year.</summary>
    string? OwnershipCompany,
    /// <summary>Year</summary>
    short? Year,
    /// <summary>Vehicle manufacturer such as Ford, Chevrolet, GMC, Ram, Toyota, etc.</summary>
    string? Make,
    /// <summary>Vehicle model such as F-150, Transit, Silverado, Express, etc.</summary>
    string? Model,
    /// <summary>Vehicle exterior color.</summary>
    string? Color,
    /// <summary>Vehicle Identification Number assigned by the manufacturer.</summary>
    string? VIN,
    /// <summary>Vehicle registration plate number.</summary>
    string? LicensePlate,
    /// <summary>State or province issuing the vehicle registration.</summary>
    string? LicensePlateState,
    /// <summary>Date the vehicle was purchased or acquired.</summary>
    DateOnly? PurchaseDate,
    /// <summary>Amount paid to acquire the vehicle.</summary>
    decimal? PurchasePrice,
    /// <summary>Indicates whether the vehicle was purchased new or used.</summary>
    string? PurchasedFrom,
    /// <summary>IsPurchasedNew</summary>
    bool? IsPurchasedNew,
    /// <summary>Internal notes and remarks regarding the vehicle.</summary>
    string? Notes,
    /// <summary>Date and time the record was created.</summary>
    DateTimeOffset CreatedOn,
    /// <summary>User who created the record.</summary>
    string? CreatedBy,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>User who last updated the record.</summary>
    string? UpdatedBy,
    /// <summary>Indicates whether the vehicle is active and available for service operations.</summary>
    bool IsActive)
;

public sealed record FgsVehicleCreateDto(
    /// <summary>Associated truck warehouse used as the vehicle inventory location.</summary>
    long WarehouseId,
    /// <summary>Indicates whether the vehicle is owned, leased, or rented.</summary>
    string? OwnershipType,
    /// <summary>Vehicle model year.</summary>
    string? OwnershipCompany,
    /// <summary>Year</summary>
    short? Year,
    /// <summary>Vehicle manufacturer such as Ford, Chevrolet, GMC, Ram, Toyota, etc.</summary>
    string? Make,
    /// <summary>Vehicle model such as F-150, Transit, Silverado, Express, etc.</summary>
    string? Model,
    /// <summary>Vehicle exterior color.</summary>
    string? Color,
    /// <summary>Vehicle Identification Number assigned by the manufacturer.</summary>
    string? VIN,
    /// <summary>Vehicle registration plate number.</summary>
    string? LicensePlate,
    /// <summary>State or province issuing the vehicle registration.</summary>
    string? LicensePlateState,
    /// <summary>Date the vehicle was purchased or acquired.</summary>
    DateOnly? PurchaseDate,
    /// <summary>Amount paid to acquire the vehicle.</summary>
    decimal? PurchasePrice,
    /// <summary>Indicates whether the vehicle was purchased new or used.</summary>
    string? PurchasedFrom,
    /// <summary>IsPurchasedNew</summary>
    bool? IsPurchasedNew,
    /// <summary>Internal notes and remarks regarding the vehicle.</summary>
    string? Notes)
;

public sealed record FgsVehicleUpdateDto(
    /// <summary>Associated truck warehouse used as the vehicle inventory location.</summary>
    long WarehouseId,
    /// <summary>Indicates whether the vehicle is owned, leased, or rented.</summary>
    string? OwnershipType,
    /// <summary>Vehicle model year.</summary>
    string? OwnershipCompany,
    /// <summary>Year</summary>
    short? Year,
    /// <summary>Vehicle manufacturer such as Ford, Chevrolet, GMC, Ram, Toyota, etc.</summary>
    string? Make,
    /// <summary>Vehicle model such as F-150, Transit, Silverado, Express, etc.</summary>
    string? Model,
    /// <summary>Vehicle exterior color.</summary>
    string? Color,
    /// <summary>Vehicle Identification Number assigned by the manufacturer.</summary>
    string? VIN,
    /// <summary>Vehicle registration plate number.</summary>
    string? LicensePlate,
    /// <summary>State or province issuing the vehicle registration.</summary>
    string? LicensePlateState,
    /// <summary>Date the vehicle was purchased or acquired.</summary>
    DateOnly? PurchaseDate,
    /// <summary>Amount paid to acquire the vehicle.</summary>
    decimal? PurchasePrice,
    /// <summary>Indicates whether the vehicle was purchased new or used.</summary>
    string? PurchasedFrom,
    /// <summary>IsPurchasedNew</summary>
    bool? IsPurchasedNew,
    /// <summary>Internal notes and remarks regarding the vehicle.</summary>
    string? Notes)
;

public sealed record FgsVehiclePatchDto(
    /// <summary>Associated truck warehouse used as the vehicle inventory location.</summary>
    long? WarehouseId,
    /// <summary>Indicates whether the vehicle is owned, leased, or rented.</summary>
    string? OwnershipType,
    /// <summary>Vehicle model year.</summary>
    string? OwnershipCompany,
    /// <summary>Year</summary>
    short? Year,
    /// <summary>Vehicle manufacturer such as Ford, Chevrolet, GMC, Ram, Toyota, etc.</summary>
    string? Make,
    /// <summary>Vehicle model such as F-150, Transit, Silverado, Express, etc.</summary>
    string? Model,
    /// <summary>Vehicle exterior color.</summary>
    string? Color,
    /// <summary>Vehicle Identification Number assigned by the manufacturer.</summary>
    string? VIN,
    /// <summary>Vehicle registration plate number.</summary>
    string? LicensePlate,
    /// <summary>State or province issuing the vehicle registration.</summary>
    string? LicensePlateState,
    /// <summary>Date the vehicle was purchased or acquired.</summary>
    DateOnly? PurchaseDate,
    /// <summary>Amount paid to acquire the vehicle.</summary>
    decimal? PurchasePrice,
    /// <summary>Indicates whether the vehicle was purchased new or used.</summary>
    string? PurchasedFrom,
    /// <summary>IsPurchasedNew</summary>
    bool? IsPurchasedNew,
    /// <summary>Internal notes and remarks regarding the vehicle.</summary>
    string? Notes)
;

