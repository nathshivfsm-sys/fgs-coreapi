using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SetupPaymentMethods;

internal static class FgsSetupPaymentMethodSql
{
    public const string Table = "setup.\"FgsSetupPaymentMethod\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "DisplayName", "SortOrder", "IsMobileVisible", "IsCustomerPortalVisible", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "DisplayName", "SortOrder", "IsMobileVisible", "IsCustomerPortalVisible", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "DisplayName", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "SortOrder", "DisplayName", "IsMobileVisible", "IsCustomerPortalVisible"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"SortOrder\" {dir} NULLS LAST, \"DisplayName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("SortOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"SortOrder\" {dir} NULLS LAST, \"DisplayName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
