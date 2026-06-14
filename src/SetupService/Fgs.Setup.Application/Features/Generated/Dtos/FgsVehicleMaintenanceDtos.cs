namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsVehicleMaintenance</summary>
public sealed record FgsVehicleMaintenanceSummaryDto(
    /// <summary>Primary key.</summary>
    long Id,
    /// <summary>Tenant identifier.</summary>
    long TenantId,
    /// <summary>Company identifier.</summary>
    long CompanyId,
    /// <summary>Vehicle that received or is scheduled to receive maintenance service.</summary>
    long VehicleId,
    /// <summary>Type of maintenance activity being performed or scheduled.</summary>
    int VehicleMaintenanceTypeId,
    /// <summary>Date the maintenance was performed or is scheduled to be performed.</summary>
    DateOnly ServiceDate,
    /// <summary>Vehicle odometer reading at the time the maintenance was performed.</summary>
    int? MileageAtService,
    /// <summary>Name of the repair shop, dealership, service provider, or maintenance vendor.</summary>
    string? ServiceProvider,
    /// <summary>Total cost incurred for the maintenance activity.</summary>
    string? InvoiceNumber,
    /// <summary>Cost</summary>
    decimal? Cost,
    /// <summary>Recommended next service date based on maintenance provider recommendations.</summary>
    DateOnly? NextServiceDate,
    /// <summary>Recommended next service mileage based on maintenance provider recommendations.</summary>
    int? NextServiceMileage,
    /// <summary>Short summary of the maintenance activity performed or scheduled.</summary>
    bool IsCompleted,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>Date and time the record was created.</summary>
    string? Notes,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn)
;

public sealed record FgsVehicleMaintenanceDetailDto(
    /// <summary>Primary key.</summary>
    long Id,
    /// <summary>Tenant identifier.</summary>
    long TenantId,
    /// <summary>Company identifier.</summary>
    long CompanyId,
    /// <summary>Vehicle that received or is scheduled to receive maintenance service.</summary>
    long VehicleId,
    /// <summary>Type of maintenance activity being performed or scheduled.</summary>
    int VehicleMaintenanceTypeId,
    /// <summary>Date the maintenance was performed or is scheduled to be performed.</summary>
    DateOnly ServiceDate,
    /// <summary>Vehicle odometer reading at the time the maintenance was performed.</summary>
    int? MileageAtService,
    /// <summary>Name of the repair shop, dealership, service provider, or maintenance vendor.</summary>
    string? ServiceProvider,
    /// <summary>Total cost incurred for the maintenance activity.</summary>
    string? InvoiceNumber,
    /// <summary>Cost</summary>
    decimal? Cost,
    /// <summary>Recommended next service date based on maintenance provider recommendations.</summary>
    DateOnly? NextServiceDate,
    /// <summary>Recommended next service mileage based on maintenance provider recommendations.</summary>
    int? NextServiceMileage,
    /// <summary>Short summary of the maintenance activity performed or scheduled.</summary>
    bool IsCompleted,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>Date and time the record was created.</summary>
    string? Notes,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>User who created the record.</summary>
    string? CreatedBy,
    /// <summary>Date and time the record was last updated.</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>User who last updated the record.</summary>
    string? UpdatedBy)
;

public sealed record FgsVehicleMaintenanceCreateDto(
    /// <summary>Vehicle that received or is scheduled to receive maintenance service.</summary>
    long VehicleId,
    /// <summary>Type of maintenance activity being performed or scheduled.</summary>
    int VehicleMaintenanceTypeId,
    /// <summary>Date the maintenance was performed or is scheduled to be performed.</summary>
    DateOnly ServiceDate,
    /// <summary>Vehicle odometer reading at the time the maintenance was performed.</summary>
    int? MileageAtService,
    /// <summary>Name of the repair shop, dealership, service provider, or maintenance vendor.</summary>
    string? ServiceProvider,
    /// <summary>Total cost incurred for the maintenance activity.</summary>
    string? InvoiceNumber,
    /// <summary>Cost</summary>
    decimal? Cost,
    /// <summary>Recommended next service date based on maintenance provider recommendations.</summary>
    DateOnly? NextServiceDate,
    /// <summary>Recommended next service mileage based on maintenance provider recommendations.</summary>
    int? NextServiceMileage,
    /// <summary>Short summary of the maintenance activity performed or scheduled.</summary>
    bool IsCompleted,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>Date and time the record was created.</summary>
    string? Notes)
;

public sealed record FgsVehicleMaintenanceUpdateDto(
    /// <summary>Vehicle that received or is scheduled to receive maintenance service.</summary>
    long VehicleId,
    /// <summary>Type of maintenance activity being performed or scheduled.</summary>
    int VehicleMaintenanceTypeId,
    /// <summary>Date the maintenance was performed or is scheduled to be performed.</summary>
    DateOnly ServiceDate,
    /// <summary>Vehicle odometer reading at the time the maintenance was performed.</summary>
    int? MileageAtService,
    /// <summary>Name of the repair shop, dealership, service provider, or maintenance vendor.</summary>
    string? ServiceProvider,
    /// <summary>Total cost incurred for the maintenance activity.</summary>
    string? InvoiceNumber,
    /// <summary>Cost</summary>
    decimal? Cost,
    /// <summary>Recommended next service date based on maintenance provider recommendations.</summary>
    DateOnly? NextServiceDate,
    /// <summary>Recommended next service mileage based on maintenance provider recommendations.</summary>
    int? NextServiceMileage,
    /// <summary>Short summary of the maintenance activity performed or scheduled.</summary>
    bool IsCompleted,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>Date and time the record was created.</summary>
    string? Notes)
;

public sealed record FgsVehicleMaintenancePatchDto(
    /// <summary>Vehicle that received or is scheduled to receive maintenance service.</summary>
    long? VehicleId,
    /// <summary>Type of maintenance activity being performed or scheduled.</summary>
    int? VehicleMaintenanceTypeId,
    /// <summary>Date the maintenance was performed or is scheduled to be performed.</summary>
    DateOnly? ServiceDate,
    /// <summary>Vehicle odometer reading at the time the maintenance was performed.</summary>
    int? MileageAtService,
    /// <summary>Name of the repair shop, dealership, service provider, or maintenance vendor.</summary>
    string? ServiceProvider,
    /// <summary>Total cost incurred for the maintenance activity.</summary>
    string? InvoiceNumber,
    /// <summary>Cost</summary>
    decimal? Cost,
    /// <summary>Recommended next service date based on maintenance provider recommendations.</summary>
    DateOnly? NextServiceDate,
    /// <summary>Recommended next service mileage based on maintenance provider recommendations.</summary>
    int? NextServiceMileage,
    /// <summary>Short summary of the maintenance activity performed or scheduled.</summary>
    bool? IsCompleted,
    /// <summary>Description</summary>
    string? Description,
    /// <summary>Date and time the record was created.</summary>
    string? Notes)
;

