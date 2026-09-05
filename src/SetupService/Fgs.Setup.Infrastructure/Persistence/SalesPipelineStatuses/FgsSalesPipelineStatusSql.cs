using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SalesPipelineStatuses;

internal static class FgsSalesPipelineStatusSql
{
    public const string Table = "setup.\"FgsSalesPipelineStatus\"";

    public const string SelectDetailColumns = """
        "Id", "StatusCode", "StatusName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "IsTerminal", "AllowManualSelection", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "StatusCode", "StatusName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "IsTerminal", "AllowManualSelection", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "StatusCode", "StatusName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "StatusCode", "StatusName", "Description", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "IsTerminal", "AllowManualSelection"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            nullsLastTiebreakerColumn: "StatusName");

}