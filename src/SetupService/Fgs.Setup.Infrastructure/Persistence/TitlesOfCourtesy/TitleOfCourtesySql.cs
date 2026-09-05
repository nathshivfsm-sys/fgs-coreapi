using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.TitlesOfCourtesy;

internal static class TitleOfCourtesySql
{
    public const string Table = "setup.\"FgsSetupTitleOfCourtesy\"";

    public const string SelectDetailColumns = """
        "Id", "Code", "DisplayName", "SortOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Code", "DisplayName", "SortOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "DisplayName", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "Code", "DisplayName", "SortOrder", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "SortOrder", nullsLastTiebreakerColumn: "DisplayName");

}