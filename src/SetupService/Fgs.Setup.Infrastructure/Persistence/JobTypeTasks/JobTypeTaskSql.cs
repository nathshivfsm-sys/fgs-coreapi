using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeTasks;

internal static class JobTypeTaskSql
{
    public const string Table = "setup.\"FgsJobTypeTask\"";

    public const string FromJoins = """
        setup."FgsJobTypeTask" t
        INNER JOIN setup."FgsSetupTechTrade" tr
            ON tr."Id" = t."TradeId"
           AND tr."TenantId" = t."TenantId"
           AND tr."CompanyId" = t."CompanyId"
        LEFT JOIN setup."FgsSetupTechSkillLevel" sl
            ON sl."Id" = t."SkillLevelId"
           AND sl."TenantId" = t."TenantId"
           AND sl."CompanyId" = t."CompanyId"
        """;

    public const string SelectDetailColumns = """
        t."Id", t."JobTypeCategoryId", t."TradeId", t."SkillLevelId", t."Name", t."TaskName",
        t."Priority", t."EstimatedHours", t."DisplayOrder", t."IsActive",
        NULL AS "CategoryName", tr."Name" AS "TradeName", sl."Name" AS "SkillName"
        """;

    public const string SelectSummaryColumns = """
        t."Id", t."JobTypeCategoryId", t."TradeId", t."SkillLevelId", t."Name", t."TaskName",
        t."Priority", t."EstimatedHours", t."DisplayOrder", t."IsActive",
        NULL AS "CategoryName", tr."Name" AS "TradeName", sl."Name" AS "SkillName"
        """;

    public const string SelectLookupColumns = """
        "Id", "Name"
        """;

    public const string AssignedToJobTypeExists = """
        EXISTS (
            SELECT 1
            FROM setup."FgsJobTypeCategory" jtc
            WHERE jtc."JobTypeTaskId" = t."Id"
              AND jtc."JobTypeId" = @JobTypeId
              AND jtc."TenantId" = @TenantId
              AND jtc."CompanyId" = @CompanyId
        )
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "JobTypeCategoryId", "TradeId", "SkillLevelId",
        "Name", "TaskName", "Priority", "EstimatedHours"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns, tableAlias: "t");
}
