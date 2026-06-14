using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsCrew : ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string CrewCode { get; set; } = null!;
    public string CrewName { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedOn { get; set; }
    public long CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public long? UpdatedBy { get; set; }
}
