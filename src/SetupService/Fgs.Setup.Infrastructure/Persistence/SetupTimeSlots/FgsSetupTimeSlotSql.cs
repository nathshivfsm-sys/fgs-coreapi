using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupTimeSlots;

internal static class FgsSetupTimeSlotSql
{
    public const string Table = "setup.\"FgsSetupTimeSlot\"";

    public const string SelectDetailColumns = """
        "Id", "FgsSetupZoneId", "Code", "Name", "BeginTime", "EndTime", "MarkTechArrivedLateAfter", "MarkWorkOrderDelayedCompletionAfter", "IsMobileVisible", "IsCustomerPortalVisible", "IncludeInCapacityPlanning", "ShowToExternalSystem", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "FgsSetupZoneId", "Code", "Name", "BeginTime", "EndTime", "MarkTechArrivedLateAfter", "MarkWorkOrderDelayedCompletionAfter", "IsMobileVisible", "IsCustomerPortalVisible", "IncludeInCapacityPlanning", "ShowToExternalSystem", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "FgsSetupZoneId", "Code", "Name", "BeginTime", "EndTime", "MarkTechArrivedLateAfter", "MarkWorkOrderDelayedCompletionAfter", "IsMobileVisible", "IsCustomerPortalVisible", "IncludeInCapacityPlanning", "ShowToExternalSystem"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}