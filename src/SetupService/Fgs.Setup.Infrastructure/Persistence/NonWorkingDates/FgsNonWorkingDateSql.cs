using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.NonWorkingDates;

internal static class FgsNonWorkingDateSql
{
    public const string Table = "setup.\"FgsNonWorkingDate\"";

    public const string SelectDetailColumns = """
        "Id", "NonWorkingDate", "Name", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "NonWorkingDate", "Name", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "NonWorkingDate", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "NonWorkingDate", "Name"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "NonWorkingDate");
}
