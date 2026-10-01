namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Optional accounting configuration for a tenant- and company-scoped billing category.
/// </summary>
public class FgsBillingCategoryAccounting : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long BillingCategoryId { get; set; }

    public long? RevenueAccountId { get; set; }

    public long? CogsAccountId { get; set; }

    public long? InventoryOffsetAccountId { get; set; }
}
