using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.FgsBusinessTypes;

internal static class FgsBusinessTypeSql
{
    public const string Table = "setup.\"FgsBusinessType\"";

    public const string SelectDetailColumns = """
        "Id", "Code", "Name", "Description", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Code", "Name", "Description", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "Code", "Name", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"Name\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}