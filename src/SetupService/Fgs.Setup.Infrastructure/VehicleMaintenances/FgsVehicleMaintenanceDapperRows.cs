using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;

namespace Fgs.Setup.Infrastructure.VehicleMaintenances;

internal sealed class FgsVehicleMaintenanceSummaryRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
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
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public FgsVehicleMaintenanceSummaryDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
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
            IsActive,
            CreatedOn,
            UpdatedOn);
}

internal sealed class FgsVehicleMaintenanceDetailRow
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
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
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    public FgsVehicleMaintenanceDetailDto ToDto() =>
        new(
            Id,
            TenantId,
            CompanyId,
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
            IsActive,
            CreatedOn,
            CreatedBy,
            UpdatedOn,
            UpdatedBy);
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
