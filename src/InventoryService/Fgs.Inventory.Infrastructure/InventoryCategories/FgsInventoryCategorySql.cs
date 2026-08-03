using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.InventoryCategories;

internal static class FgsInventoryCategorySql
{
    public const string Table = "inventory.\"FgsInventoryCategory\"";

    public const string SelectDetailColumns = """
        "Id", "CategoryCode", "Name", "Description", "TextColor", "BackgroundColor", "DisplayIconFileId", "DisplayOrder", "IsSystem", "IsActive"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        "Id", "CategoryCode", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "CategoryCode", "Name", "DisplayOrder", "IsSystem"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir}, \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
