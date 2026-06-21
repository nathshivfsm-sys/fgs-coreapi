namespace Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;

public sealed record FgsVehicleMaintenanceSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    long VehicleId,
    int VehicleMaintenanceTypeId,
    DateOnly ServiceDate,
    int? MileageAtService,
    string? ServiceProvider,
    string? InvoiceNumber,
    decimal? Cost,
    DateOnly? NextServiceDate,
    int? NextServiceMileage,
    bool IsCompleted,
    string? Description,
    string? Notes,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsVehicleMaintenanceDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    long VehicleId,
    int VehicleMaintenanceTypeId,
    DateOnly ServiceDate,
    int? MileageAtService,
    string? ServiceProvider,
    string? InvoiceNumber,
    decimal? Cost,
    DateOnly? NextServiceDate,
    int? NextServiceMileage,
    bool IsCompleted,
    string? Description,
    string? Notes,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsVehicleMaintenanceLookupDto(
    long Id,
    long VehicleId,
    DateOnly ServiceDate);

public sealed record FgsVehicleMaintenanceCreateDto(
    long VehicleId,
    int VehicleMaintenanceTypeId,
    DateOnly ServiceDate,
    int? MileageAtService,
    string? ServiceProvider,
    string? InvoiceNumber,
    decimal? Cost,
    DateOnly? NextServiceDate,
    int? NextServiceMileage,
    bool IsCompleted,
    string? Description,
    string? Notes);

public sealed record FgsVehicleMaintenanceUpdateDto(
    long VehicleId,
    int VehicleMaintenanceTypeId,
    DateOnly ServiceDate,
    int? MileageAtService,
    string? ServiceProvider,
    string? InvoiceNumber,
    decimal? Cost,
    DateOnly? NextServiceDate,
    int? NextServiceMileage,
    bool IsCompleted,
    string? Description,
    string? Notes);

public sealed record FgsVehicleMaintenancePatchDto(
    long? VehicleId,
    int? VehicleMaintenanceTypeId,
    DateOnly? ServiceDate,
    int? MileageAtService,
    string? ServiceProvider,
    string? InvoiceNumber,
    decimal? Cost,
    DateOnly? NextServiceDate,
    int? NextServiceMileage,
    bool? IsCompleted,
    string? Description,
    string? Notes,
    bool? IsActive);

public sealed record FgsVehicleMaintenanceListFilters(
    bool? IsCompleted = null,
    long? VehicleId = null);
