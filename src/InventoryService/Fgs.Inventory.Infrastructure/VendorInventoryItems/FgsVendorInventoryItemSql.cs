using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.VendorInventoryItems;

internal static class FgsVendorInventoryItemSql
{
    public const string Table = "inventory.\"FgsVendorInventoryItem\"";

    public const string InventoryItemTable = "inventory.\"FgsInventoryItem\"";

    public const string SelectDetailColumns = """
        "Id", "VendorId", "InventoryItemId", "VendorPartNumber", "VendorPartName", "LastCost", "LastReceivedDate",
        "PurchaseOrderComments", "VendorPriority", "LeadTimeDays", "IsActive"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        "Id", "VendorId", "InventoryItemId", "VendorPartNumber"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "VendorId", "InventoryItemId", "VendorPartNumber", "VendorPartName", "LastCost",
        "LastReceivedDate", "VendorPriority", "LeadTimeDays"
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
