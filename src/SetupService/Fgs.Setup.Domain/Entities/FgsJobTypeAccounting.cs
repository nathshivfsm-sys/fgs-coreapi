namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Optional accounting configuration that applies at the job type level independently of billing category.
/// </summary>
public class FgsJobTypeAccounting : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long JobTypeId { get; set; }

    public long? ArAccountId { get; set; }

    public long? DiscountAccountId { get; set; }
}
