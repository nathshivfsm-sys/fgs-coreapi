using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class CrmServiceLocation : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long CustomerId { get; set; }

    public int LocationSequence { get; set; }

    public string LocationNumber { get; set; } = null!;
}
