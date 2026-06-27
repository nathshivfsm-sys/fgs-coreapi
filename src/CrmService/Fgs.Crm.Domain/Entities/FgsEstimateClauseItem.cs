using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class FgsEstimateClauseItem : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long EstimateId { get; set; }

    public long? ClauseId { get; set; }

    public long ClauseTypeId { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public string ClauseName { get; set; } = null!;

    public string ClauseText { get; set; } = null!;

    public bool ShowOnProposal { get; set; } = true;
}
