using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.EntityDefaultTermsConditions;

internal static class FgsEntityDefaultTermsConditionSql
{
    public const string Table = "setup.\"FgsEntityDefaultTermsCondition\"";
    public const string TermsConditionTable = "setup.\"FgsTermsCondition\"";

    public const string SelectDetailColumns = """
        d."Id", d."EntityType", d."TermsConditionId",
        t."Code" AS "TermsConditionCode", t."Name" AS "TermsConditionName", t."VersionNumber" AS "TermsConditionVersionNumber",
        d."IsActive"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        d."Id", d."EntityType", d."TermsConditionId"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "EntityType", "TermsConditionId"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "EntityType",
            tableAlias: "d");
}
