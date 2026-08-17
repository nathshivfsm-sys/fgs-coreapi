using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.LeadStatuses;

internal static class LeadStatusSql
{
    public const string Table = "setup.\"FgsLeadStatus\"";

    public const string SelectDetailColumns = """
        "Id", "StatusCode", "StatusName", "Description", "DisplayOrder", "IsSystem", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "StatusCode", "StatusName", "Description", "DisplayOrder", "IsSystem", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "StatusCode", "StatusName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "StatusCode", "StatusName", "Description", "IsSystem"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            nullsLastTiebreakerColumn: "StatusName");

}