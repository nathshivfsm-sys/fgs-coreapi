using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class CrmEntityTag : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long TagId { get; set; }

    public int EntityTypeId { get; set; }

    public long EntityId { get; set; }

    public short DisplayOrder { get; set; } = 1;
}
