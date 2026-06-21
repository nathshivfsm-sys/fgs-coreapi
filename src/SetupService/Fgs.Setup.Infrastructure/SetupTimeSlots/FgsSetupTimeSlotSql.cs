using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SetupTimeSlots;

internal static class FgsSetupTimeSlotSql
{
    public const string Table = "setup.\"FgsSetupTimeSlot\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "FgsSetupZoneId", "Code", "Name", "BeginTime", "EndTime", "MarkTechArrivedLateAfter", "MarkWorkOrderDelayedCompletionAfter", "IsMobileVisible", "IsCustomerPortalVisible", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "FgsSetupZoneId", "Code", "Name", "BeginTime", "EndTime", "MarkTechArrivedLateAfter", "MarkWorkOrderDelayedCompletionAfter", "IsMobileVisible", "IsCustomerPortalVisible", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "FgsSetupZoneId", "Code", "Name", "BeginTime", "EndTime", "MarkTechArrivedLateAfter", "MarkWorkOrderDelayedCompletionAfter", "IsMobileVisible", "IsCustomerPortalVisible"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Name\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"Name\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
