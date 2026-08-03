namespace Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;

public sealed record FgsTenantServiceAccountsSetupDetailDto(
    long TenantId,
    long CompanyId,
    long? BankAccountId,
    long? AccountsReceivableAccountId,
    long? RevenueAccountId,
    long? DiscountAccountId,
    long? SalesTaxPayableAccountId,
    long? InventoryAccountId,
    long? COGSAccountId,
    long? UndepositedFundsAccountId,
    long? ProcessingFeeAccountId,
    long? AccountsPayableAccountId,
    bool IsActive);

public sealed record FgsTenantServiceAccountsSetupUpdateDto(
    long? BankAccountId,
    long? AccountsReceivableAccountId,
    long? RevenueAccountId,
    long? DiscountAccountId,
    long? SalesTaxPayableAccountId,
    long? InventoryAccountId,
    long? COGSAccountId,
    long? UndepositedFundsAccountId,
    long? ProcessingFeeAccountId,
    long? AccountsPayableAccountId,
    bool IsActive);

public sealed record FgsTenantServiceAccountsSetupPatchDto(
    long? BankAccountId = null,
    long? AccountsReceivableAccountId = null,
    long? RevenueAccountId = null,
    long? DiscountAccountId = null,
    long? SalesTaxPayableAccountId = null,
    long? InventoryAccountId = null,
    long? COGSAccountId = null,
    long? UndepositedFundsAccountId = null,
    long? ProcessingFeeAccountId = null,
    long? AccountsPayableAccountId = null,
    bool? IsActive = null);
