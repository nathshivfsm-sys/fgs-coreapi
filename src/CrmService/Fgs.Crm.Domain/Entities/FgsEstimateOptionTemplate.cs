using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsEstimateOptionTemplate : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long EstimateOptionId { get; set; }

    public long EstimateTemplateId { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }
}
