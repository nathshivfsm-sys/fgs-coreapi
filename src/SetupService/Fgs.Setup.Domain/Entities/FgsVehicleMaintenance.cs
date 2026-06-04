namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Completed or scheduled maintenance activity for a company vehicle.
/// </summary>
public class FgsVehicleMaintenance : FgsEntityBase, ITenantCompanyScoped
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

    public bool IsCompleted { get; set; } = true;

    public string? Description { get; set; }

    public string? Notes { get; set; }
}
