using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SalesDispositionReasons;

internal static class FgsSalesDispositionReasonSql
{
    public const string Table = "setup.\"FgsSalesDispositionReason\"";

    public const string SelectDetailColumns = """
        "Id", "DispositionReasonCode", "DispositionReasonName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "RequireComment", "IsTerminal", "AllowManualSelection", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "DispositionReasonCode", "DispositionReasonName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "RequireComment", "IsTerminal", "AllowManualSelection", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "DispositionReasonCode", "DispositionReasonName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "DispositionReasonCode", "DispositionReasonName", "Description", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "RequireComment", "IsTerminal", "AllowManualSelection"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            nullsLastTiebreakerColumn: "DispositionReasonName");

}