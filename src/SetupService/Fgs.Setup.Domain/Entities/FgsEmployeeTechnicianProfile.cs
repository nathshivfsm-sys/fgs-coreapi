using Fgs.Kernel.Entities;

namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Technician-specific operational settings for dispatching, scheduling, and routing.
/// </summary>
public class FgsEmployeeTechnicianProfile : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long EmployeeId { get; set; }

    public string TechCode { get; set; } = null!;

    public string? TechName { get; set; }

    public bool CanBeScheduled { get; set; } = true;

    public decimal DailyCapacityHours { get; set; } = 8.00m;

    public long? DispatchZoneId { get; set; }

    public short StartLocationTypeId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public int? TechTradeId { get; set; }

    public int? TechSkillId { get; set; }

    public long? TruckId { get; set; }

    public string? CustomerFacingPhone { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedOn { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public long? UpdatedBy { get; set; }
}
