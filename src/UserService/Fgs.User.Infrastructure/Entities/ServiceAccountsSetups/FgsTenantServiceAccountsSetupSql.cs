using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.ServiceAccountsSetups;

internal static class FgsTenantServiceAccountsSetupSql
{
    public const string Table = "tenant.\"FgsTenantServiceAccountsSetup\"";

    public const string SelectDetailColumns = """
        "TenantId", "CompanyId", "BankAccountId", "AccountsReceivableAccountId", "RevenueAccountId",
        "DiscountAccountId", "SalesTaxPayableAccountId", "InventoryAccountId", "COGSAccountId",
        "UndepositedFundsAccountId", "ProcessingFeeAccountId", "AccountsPayableAccountId", "IsActive"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        "CompanyId", "IsActive"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "CompanyId",
        "BankAccountId",
        "AccountsReceivableAccountId",
        "RevenueAccountId",
        "DiscountAccountId",
        "SalesTaxPayableAccountId",
        "InventoryAccountId",
        "COGSAccountId",
        "UndepositedFundsAccountId",
        "ProcessingFeeAccountId",
        "AccountsPayableAccountId",
        "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"CompanyId\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
