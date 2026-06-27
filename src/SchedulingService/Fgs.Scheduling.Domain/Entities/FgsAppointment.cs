using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsAppointment : ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public short SourceTypeId { get; set; }
    public long SourceId { get; set; }
    public long? CrewId { get; set; }
    public string? CustomerContactName { get; set; }
    public DateOnly ServiceDate { get; set; }
    public TimeOnly ScheduledTime { get; set; }
    public decimal EstimatedHours { get; set; }
    public short AppointmentStatusId { get; set; }
    public DateTimeOffset? CustomerApprovedOn { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public long CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public long? UpdatedBy { get; set; }
}
