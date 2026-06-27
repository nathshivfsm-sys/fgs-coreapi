using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.InventoryLocations;

internal static class FgsInventoryLocationSql
{
    public const string Table = "inventory.\"FgsInventoryLocation\"";

    public const string SelectDetailColumns = """
        "Id", "InventoryLocationCode", "Name", "InventoryLocationType", "ParentInventoryLocationId", "Description",
        "Address1", "Address2", "City", "StateProvince", "PostalCode", "Country",
        "ContactName", "PhoneNumber", "Email", "IsDefault", "IsActive"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        "Id", "InventoryLocationCode", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "InventoryLocationCode", "Name", "InventoryLocationType", "ParentInventoryLocationId",
        "Description", "Address1", "City", "StateProvince", "PostalCode", "Country", "ContactName", "IsDefault"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Name\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
