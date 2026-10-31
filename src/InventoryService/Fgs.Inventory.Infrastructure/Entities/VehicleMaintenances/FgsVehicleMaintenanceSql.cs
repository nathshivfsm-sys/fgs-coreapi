using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.Entities.VehicleMaintenances;

internal static class FgsVehicleMaintenanceSql
{
    public const string Table = "inventory.\"FgsVehicleMaintenance\"";

    public const string SelectDetailColumns = """
        "Id", "VehicleId", "VehicleMaintenanceTypeId", "ServiceDate", "MileageAtService", "ServiceProvider", "InvoiceNumber", "Cost", "NextServiceDate", "NextServiceMileage", "IsCompleted", "Description", "Notes", "IsActive"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        "Id", "VehicleId", "ServiceDate"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "VehicleId", "VehicleMaintenanceTypeId", "ServiceDate", "MileageAtService", "ServiceProvider", "InvoiceNumber", "Cost", "NextServiceDate", "NextServiceMileage", "IsCompleted", "Description", "Notes"
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
