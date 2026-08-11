using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.Vehicles;

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
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}