using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SalesPipelineStatuses;

internal static class FgsSalesPipelineStatusSql
{
    public const string Table = "setup.\"FgsSalesPipelineStatus\"";

    public const string SelectDetailColumns = """
        "Id", "StatusCode", "StatusName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "IsTerminal", "AllowManualSelection", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "StatusCode", "StatusName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "IsTerminal", "AllowManualSelection", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "StatusCode", "StatusName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "StatusCode", "StatusName", "Description", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "IsTerminal", "AllowManualSelection"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"StatusName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"StatusName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
