using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.LeadDisqualificationReasons;

internal static class LeadDisqualificationReasonSql
{
    public const string Table = "setup.\"FgsLeadDisqualificationReason\"";

    public const string SelectDetailColumns = """
        "Id", "ReasonCode", "ReasonName", "Description", "DisplayOrder", "IsSystem", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "ReasonCode", "ReasonName", "Description", "DisplayOrder", "IsSystem", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "ReasonCode", "ReasonName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "ReasonCode", "ReasonName", "Description", "IsSystem"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            nullsLastTiebreakerColumn: "ReasonName");

}