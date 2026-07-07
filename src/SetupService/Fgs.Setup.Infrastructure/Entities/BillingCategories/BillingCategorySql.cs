using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Entities.BillingCategories;

internal static class BillingCategorySql
{
    public const string Table = "setup.\"FgsBillingCategory\"";

    public const string SelectDetailColumns = """
        "Id", "BillingCategoryType", "BillingCategoryName", "Description", "DisplayOrder", "IsSystemDefined", "ShowToFieldTech", "AllowToPick", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "BillingCategoryType", "BillingCategoryName", "Description", "DisplayOrder", "IsSystemDefined", "ShowToFieldTech", "AllowToPick", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "BillingCategoryType", "BillingCategoryName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "BillingCategoryType", "BillingCategoryName", "Description", "IsSystemDefined", "ShowToFieldTech", "AllowToPick"
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
            ? $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"BillingCategoryName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}