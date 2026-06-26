using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.SetupPostalCodes;

internal static class FgsSetupPostalCodeSql
{
    public const string Table = "setup.\"FgsSetupPostalCode\"";

    public const string SelectDetailColumns = """
        "Id", "PostalCode", "FgsSetupZoneId", "FgsSetupTaxId", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "PostalCode", "FgsSetupZoneId", "FgsSetupTaxId", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "PostalCode"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "PostalCode", "FgsSetupZoneId", "FgsSetupTaxId"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"PostalCode\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"PostalCode\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
