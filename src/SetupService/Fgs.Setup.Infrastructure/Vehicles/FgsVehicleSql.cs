using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Vehicles;

internal static class FgsVehicleSql
{
    public const string Table = "setup.\"FgsVehicle\"";

    public const string SelectDetailColumns = """
        "Id", "InventoryLocationId", "OwnershipType", "OwnershipCompany", "Year", "Make", "Model", "Color", "VIN", "LicensePlate", "LicensePlateState", "PurchaseDate", "PurchasePrice", "PurchasedFrom", "IsPurchasedNew", "Notes", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "InventoryLocationId", "OwnershipType", "OwnershipCompany", "Year", "Make", "Model", "Color", "VIN", "LicensePlate", "LicensePlateState", "PurchaseDate", "PurchasePrice", "PurchasedFrom", "IsPurchasedNew", "Notes", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Make", "Model", "VIN"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "InventoryLocationId", "OwnershipType", "OwnershipCompany", "Year", "Make", "Model", "Color", "VIN", "LicensePlate", "LicensePlateState", "PurchaseDate", "PurchasePrice", "PurchasedFrom", "IsPurchasedNew", "Notes"
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
            ? $"ORDER BY \"VIN\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}