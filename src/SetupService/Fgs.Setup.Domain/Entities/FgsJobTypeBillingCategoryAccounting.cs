namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Optional accounting overrides for a specific job type and billing category combination.
/// </summary>
public class FgsJobTypeBillingCategoryAccounting : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long JobTypeId { get; set; }

    public long BillingCategoryId { get; set; }

    public long? RevenueAccountId { get; set; }

    public long? CogsAccountId { get; set; }

    public long? InventoryOffsetAccountId { get; set; }
}
