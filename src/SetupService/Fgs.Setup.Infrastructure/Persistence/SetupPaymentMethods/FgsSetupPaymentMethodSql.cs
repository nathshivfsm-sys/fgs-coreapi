using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPaymentMethods;

internal static class FgsSetupPaymentMethodSql
{
    public const string Table = "setup.\"FgsSetupPaymentMethod\"";

    public const string SelectDetailColumns = """
        "Id", "DisplayName", "SortOrder", "IsMobileVisible", "IsCustomerPortalVisible", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "DisplayName", "SortOrder", "IsMobileVisible", "IsCustomerPortalVisible", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "DisplayName", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "SortOrder", "DisplayName", "IsMobileVisible", "IsCustomerPortalVisible"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("SortOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"SortOrder\" {dir} NULLS LAST, \"DisplayName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}