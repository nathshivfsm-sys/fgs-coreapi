using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.InventorySerials;

internal static class FgsInventorySerialSql
{
    public const string Table = "inventory.\"FgsInventorySerial\"";

    public const string InventoryItemTable = "inventory.\"FgsInventoryItem\"";

    public const string SelectDetailColumns = """
        "Id", "InventoryItemId", "SerialNumber", "InventorySerialStatus"::text AS "InventorySerialStatus", "Notes",
        "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        "Id", "SerialNumber", "InventoryItemId", "InventorySerialStatus"::text AS "InventorySerialStatus"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "InventoryItemId", "SerialNumber", "InventorySerialStatus", "CreatedOn"
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
