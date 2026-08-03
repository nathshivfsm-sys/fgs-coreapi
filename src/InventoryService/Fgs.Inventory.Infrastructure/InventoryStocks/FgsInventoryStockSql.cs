using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.InventoryStocks;

internal static class FgsInventoryStockSql
{
    public const string Table = "inventory.\"FgsInventoryStock\"";

    public const string InventoryItemTable = "inventory.\"FgsInventoryItem\"";

    public const string SelectDetailColumns = """
        "Id", "InventoryItemId", "QuantityOnHand", "QuantityCommitted", "QuantityAvailable", "AverageCost", "LastCost",
        "LastPurchaseDate", "LastSoldDate", "UpdatedOn"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "InventoryItemId", "QuantityOnHand", "QuantityCommitted", "QuantityAvailable", "AverageCost", "LastCost",
        "LastPurchaseDate", "LastSoldDate", "UpdatedOn"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
