namespace Fgs.User.Infrastructure.Entities.ServiceAccountsSetups;

internal static class FgsTenantServiceAccountsSetupSql
{
    public const string Table = "tenant.\"FgsTenantServiceAccountsSetup\"";

    public const string SelectDetailColumns = """
        "TenantId", "CompanyId", "BankAccountId", "AccountsReceivableAccountId", "RevenueAccountId",
        "DiscountAccountId", "SalesTaxPayableAccountId", "InventoryAccountId", "COGSAccountId",
        "UndepositedFundsAccountId", "ProcessingFeeAccountId", "AccountsPayableAccountId", "IsActive"
        """;
}
