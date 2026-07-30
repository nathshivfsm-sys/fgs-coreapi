using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.TruckStockTemplateItems;

internal static class FgsTruckStockTemplateItemSql
{
    public const string Table = "inventory.\"FgsTruckStockTemplateItem\"";
    public const string TemplateTable = "inventory.\"FgsTruckStockTemplate\"";
    public const string InventoryItemTable = "inventory.\"FgsInventoryItem\"";

    public const string SelectDetailColumns = """
        "Id", "TruckStockTemplateId", "InventoryItemId", "TargetQuantity", "MinimumQuantity", "DisplayOrder"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "TruckStockTemplateId", "InventoryItemId", "TargetQuantity", "MinimumQuantity", "DisplayOrder"
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
