using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.InventoryTransactions;

internal static class FgsInventoryTransactionSql
{
    public const string Table = "inventory.\"FgsInventoryTransaction\"";

    public const string InventoryItemTable = "inventory.\"FgsInventoryItem\"";

    public const string SelectDetailColumns = """
        "Id", "TransactionNumber", "InventoryItemId", "SerialNumber", "TransactionType", "Quantity", "FromInventoryLocationId",
        "ToInventoryLocationId", "UnitCost", "TransactionDate", "ReferenceType", "ReferenceId", "Notes",
        "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "TransactionNumber", "InventoryItemId", "SerialNumber", "TransactionType", "Quantity", "FromInventoryLocationId",
        "ToInventoryLocationId", "UnitCost", "TransactionDate", "ReferenceType", "ReferenceId", "CreatedOn"
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
