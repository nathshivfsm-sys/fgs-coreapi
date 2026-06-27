using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.LeadSources;

internal static class LeadSourceSql
{
    public const string Table = "setup.\"FgsLeadSource\"";

    public const string SelectDetailColumns = """
        "Id", "SourceCode", "SourceName", "Description", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "SourceCode", "SourceName", "Description", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "SourceCode", "SourceName"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "SourceCode", "SourceName", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"SourceName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"SourceName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
