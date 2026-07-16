using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.VehicleMaintenances;

internal sealed class FgsVehicleMaintenanceSummaryRow
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public int VehicleMaintenanceTypeId { get; set; }
    public DateOnly ServiceDate { get; set; }
    public int? MileageAtService { get; set; }
    public string? ServiceProvider { get; set; }
    public string? InvoiceNumber { get; set; }
    public decimal? Cost { get; set; }
    public DateOnly? NextServiceDate { get; set; }
    public int? NextServiceMileage { get; set; }
    public bool IsCompleted { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }

    public FgsVehicleMaintenanceSummaryDto ToDto() =>
        new(
            Id,
            VehicleId,
            VehicleMaintenanceTypeId,
            ServiceDate,
            MileageAtService,
            ServiceProvider,
            InvoiceNumber,
            Cost,
            NextServiceDate,
            NextServiceMileage,
            IsCompleted,
            Description,
            Notes,
            IsActive);
}

internal sealed class FgsVehicleMaintenanceDetailRow
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public int VehicleMaintenanceTypeId { get; set; }
    public DateOnly ServiceDate { get; set; }
    public int? MileageAtService { get; set; }
    public string? ServiceProvider { get; set; }
    public string? InvoiceNumber { get; set; }
    public decimal? Cost { get; set; }
    public DateOnly? NextServiceDate { get; set; }
    public int? NextServiceMileage { get; set; }
    public bool IsCompleted { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }

    public FgsVehicleMaintenanceDetailDto ToDto() =>
        new(
            Id,
            VehicleId,
            VehicleMaintenanceTypeId,
            ServiceDate,
            MileageAtService,
            ServiceProvider,
            InvoiceNumber,
            Cost,
            NextServiceDate,
            NextServiceMileage,
            IsCompleted,
            Description,
            Notes,
            IsActive);
}

internal sealed class FgsVehicleMaintenanceLookupRow
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public DateOnly ServiceDate { get; set; }

    public FgsVehicleMaintenanceLookupDto ToDto() => new(Id,
            VehicleId,
            ServiceDate);
}
