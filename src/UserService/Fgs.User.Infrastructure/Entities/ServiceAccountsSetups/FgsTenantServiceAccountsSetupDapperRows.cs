using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;

namespace Fgs.User.Infrastructure.Entities.ServiceAccountsSetups;

internal sealed class FgsTenantServiceAccountsSetupDetailRow
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
    public bool IsActive { get; set; }

    public FgsTenantServiceAccountsSetupDetailDto ToDto() =>
        new(
            TenantId,
            CompanyId,
            BankAccountId,
            AccountsReceivableAccountId,
            RevenueAccountId,
            DiscountAccountId,
            SalesTaxPayableAccountId,
            InventoryAccountId,
            COGSAccountId,
            UndepositedFundsAccountId,
            ProcessingFeeAccountId,
            AccountsPayableAccountId,
            IsActive);
}
