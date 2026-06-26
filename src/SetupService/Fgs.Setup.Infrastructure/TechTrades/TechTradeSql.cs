using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.TechTrades;

internal static class TechTradeSql
{
    public const string Table = "setup.\"FgsSetupTechTrade\"";

    public const string SelectDetailColumns = """
        "Id", "TradeCode", "Name", "Description", "SortOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TradeCode", "Name", "SortOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "TradeCode", "Name", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "TradeCode", "Name", "SortOrder", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"SortOrder\" {dir} NULLS LAST, \"Name\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("SortOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"{column}\" {dir} NULLS LAST, \"Name\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
