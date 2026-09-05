using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.VehicleMaintenances;

internal static class FgsVehicleMaintenanceSql
{
    public const string Table = "setup.\"FgsVehicleMaintenance\"";

    public const string SelectDetailColumns = """
        "Id", "VehicleId", "VehicleMaintenanceTypeId", "ServiceDate", "MileageAtService", "ServiceProvider", "InvoiceNumber", "Cost", "NextServiceDate", "NextServiceMileage", "IsCompleted", "Description", "Notes", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "VehicleId", "VehicleMaintenanceTypeId", "ServiceDate", "MileageAtService", "ServiceProvider", "InvoiceNumber", "Cost", "NextServiceDate", "NextServiceMileage", "IsCompleted", "Description", "Notes", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "VehicleId", "ServiceDate"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "VehicleId", "VehicleMaintenanceTypeId", "ServiceDate", "MileageAtService", "ServiceProvider", "InvoiceNumber", "Cost", "NextServiceDate", "NextServiceMileage", "IsCompleted", "Description", "Notes"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}