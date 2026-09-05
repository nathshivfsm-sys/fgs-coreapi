using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.TermsConditions;

internal static class FgsTermsConditionSql
{
    public const string Table = "setup.\"FgsTermsCondition\"";

    public const string SelectDetailColumns = """
        "Id", "Code", "Name", "VersionNumber", "TermsText", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Code", "Name", "VersionNumber", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name", "VersionNumber"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "Code", "Name", "VersionNumber"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "Code");
}
