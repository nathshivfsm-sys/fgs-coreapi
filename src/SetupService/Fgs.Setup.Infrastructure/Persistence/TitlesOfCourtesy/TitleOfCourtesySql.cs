using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.TitlesOfCourtesy;

internal static class TitleOfCourtesySql
{
    public const string Table = "setup.\"FgsSetupTitleOfCourtesy\"";

    public const string SelectDetailColumns = """
        "Id", "Code", "DisplayName", "SortOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Code", "DisplayName", "SortOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "DisplayName", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "Code", "DisplayName", "SortOrder", "IsActive"
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
            ? $"ORDER BY \"{column}\" {dir} NULLS LAST, \"DisplayName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}