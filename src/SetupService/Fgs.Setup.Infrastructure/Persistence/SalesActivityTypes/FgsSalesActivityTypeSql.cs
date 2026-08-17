using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SalesActivityTypes;

internal static class FgsSalesActivityTypeSql
{
    public const string Table = "setup.\"FgsSalesActivityType\"";

    public const string SelectDetailColumns = """
        "Id", "ActivityTypeCode", "ActivityTypeName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "AllowManualSelection", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "ActivityTypeCode", "ActivityTypeName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "AllowManualSelection", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "ActivityTypeCode", "ActivityTypeName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "ActivityTypeCode", "ActivityTypeName", "Description", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "AllowManualSelection"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            nullsLastTiebreakerColumn: "ActivityTypeName");

}