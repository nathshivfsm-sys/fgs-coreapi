using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsAppointmentAssignment : ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long AppointmentId { get; set; }
    public long EmployeeId { get; set; }
    public long? CrewId { get; set; }
    public DateOnly ServiceDate { get; set; }
    public TimeOnly ScheduledTime { get; set; }
    public decimal EstimatedHours { get; set; }
    public DateTimeOffset? ActualStartOn { get; set; }
    public DateTimeOffset? ActualEndOn { get; set; }
    public DateTimeOffset AssignedOn { get; set; }
    public long AssignedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public long CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public long? UpdatedBy { get; set; }
}
