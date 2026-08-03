using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.InventoryItemTypes;

internal static class FgsInventoryItemTypeSql
{
    public const string Table = "inventory.\"FgsInventoryItemType\"";

    public const string SelectDetailColumns = """
        "Id", "ItemTypeCode", "Name", "Description", "TracksQuantity", "DisplayOrder", "IsSystem", "IsActive"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        "Id", "ItemTypeCode", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "ItemTypeCode", "Name", "DisplayOrder", "TracksQuantity", "IsSystem"
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
