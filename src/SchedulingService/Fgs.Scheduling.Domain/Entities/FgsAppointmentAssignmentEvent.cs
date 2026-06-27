using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsAppointmentAssignmentEvent : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long? AssignmentId { get; set; }
    public long EmployeeId { get; set; }
    public DateOnly ServiceDate { get; set; }
    public short EventTypeId { get; set; }
    public DateTimeOffset EventOccurredOn { get; set; }
    public bool EnteredByOffice { get; set; }
    public string? Notes { get; set; }
}
