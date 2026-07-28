using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.JobCategories;

internal static class JobCategorySql
{
    public const string Table = "setup.\"FgsJobCategory\"";

    public const string SelectDetailColumns = """
        "Id", "CategoryCode", "Name", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "CategoryCode", "Name", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "CategoryCode", "Name", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "CategoryCode", "Name"
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
            ? $"ORDER BY \"Id\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
