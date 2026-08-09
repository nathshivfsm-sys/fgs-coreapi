using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeCategories;

internal static class JobTypeCategorySql
{
    public const string Table = "setup.\"FgsJobTypeCategory\"";

    public const string SelectDetailColumns = """
        "Id", "JobTypeId", "JobCategoryId", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "JobTypeId", "JobCategoryId", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "JobTypeId", "JobCategoryId", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "JobTypeId", "JobCategoryId"
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
