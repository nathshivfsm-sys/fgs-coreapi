namespace Fgs.User.Domain.Entities;

/// <summary>
/// Per-company default general ledger account mappings used when posting invoices, payments, and inventory transactions.
/// </summary>
public class FgsTenantServiceAccountsSetup : FgsEntityBase, ITenantCompanyScoped
{
    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long? BankAccountId { get; set; }

    public long? AccountsReceivableAccountId { get; set; }

    public long? RevenueAccountId { get; set; }

    public long? DiscountAccountId { get; set; }

    public long? SalesTaxPayableAccountId { get; set; }

    public long? InventoryAccountId { get; set; }

    public long? COGSAccountId { get; set; }

    public long? UndepositedFundsAccountId { get; set; }

    public long? ProcessingFeeAccountId { get; set; }

    public long? AccountsPayableAccountId { get; set; }

    public bool IsActive { get; set; } = true;
}
