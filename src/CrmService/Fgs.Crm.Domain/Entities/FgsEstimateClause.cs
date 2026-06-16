using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsEstimateClause : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long ClauseTypeId { get; set; }

    public string ClauseName { get; set; } = null!;

    public string ClauseText { get; set; } = null!;

    public short DisplayOrder { get; set; } = 1;

    public bool IsActive { get; set; } = true;
}
