using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.InventoryItems;

internal static class FgsInventoryItemSql
{
    public const string Table = "inventory.\"FgsInventoryItem\"";
    public const string AlternateTable = "inventory.\"FgsInventoryItemAlternate\"";
    public const string DependencyTable = "inventory.\"FgsInventoryItemDependency\"";

    public const string SelectDetailColumns = """
        "Id", "InventoryItemTypeId", "InventoryCategoryId", "InventorySubCategoryId",
        "ItemCode", "Name", "Description", "PurchaseDescription", "SalesDescription",
        "ManufacturerPartNumber", "ManufacturerName", "Sku", "UPCCode", "UnitOfMeasure",
        "TracksInventory", "IsSerialized", "UnitCost", "StandardUnitCost", "SalesPrice", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "ItemCode", "Name", "InventoryItemTypeId", "InventoryCategoryId", "InventorySubCategoryId",
        "TracksInventory", "IsSerialized", "UnitCost", "SalesPrice", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "ItemCode", "Name"
        """;

    public const string SelectAlternateColumns = """
        "Id", "AlternateInventoryItemId", "PriorityOrder", "Notes", "IsActive"
        """;

    public const string SelectDependencyColumns = """
        "Id", "DependentInventoryItemId", "Quantity", "IsRequired", "Notes", "DisplayOrder", "IsActive"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "ItemCode", "Name", "InventoryItemTypeId", "TracksInventory", "UnitCost", "SalesPrice"
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
