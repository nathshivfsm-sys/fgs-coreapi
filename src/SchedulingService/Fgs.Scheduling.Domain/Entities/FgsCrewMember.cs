using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsCrewMember : ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long CrewId { get; set; }
    public long EmployeeId { get; set; }
    public bool IsLead { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public long CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public long? UpdatedBy { get; set; }
}
