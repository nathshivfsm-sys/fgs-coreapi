using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsEstimateStatus : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public string StatusCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public short DisplayOrder { get; set; } = 1;

    public bool IsActive { get; set; } = true;
}
