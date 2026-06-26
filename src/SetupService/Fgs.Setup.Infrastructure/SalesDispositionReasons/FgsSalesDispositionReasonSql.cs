using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.SalesDispositionReasons;

internal static class FgsSalesDispositionReasonSql
{
    public const string Table = "setup.\"FgsSalesDispositionReason\"";

    public const string SelectDetailColumns = """
        "Id", "DispositionReasonCode", "DispositionReasonName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "RequireComment", "IsTerminal", "AllowManualSelection", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "DispositionReasonCode", "DispositionReasonName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "RequireComment", "IsTerminal", "AllowManualSelection", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "DispositionReasonCode", "DispositionReasonName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "DispositionReasonCode", "DispositionReasonName", "Description", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "RequireComment", "IsTerminal", "AllowManualSelection"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"DispositionReasonName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"DispositionReasonName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
