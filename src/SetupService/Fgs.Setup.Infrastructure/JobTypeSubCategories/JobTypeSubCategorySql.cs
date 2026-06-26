using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.JobTypeSubCategories;

internal static class JobTypeSubCategorySql
{
    public const string Table = "setup.\"FgsJobTypeSubCategory\"";

    public const string SelectDetailColumns = """
        "Id", "SubCategoryCode", "Name", "Description", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "SubCategoryCode", "Name", "Description", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "SubCategoryCode", "Name", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "SubCategoryCode", "Name", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"Name\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"Name\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
