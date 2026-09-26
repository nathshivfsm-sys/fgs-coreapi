using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeTasks;

internal static class JobTypeTaskSql
{
    public const string Table = "setup.\"FgsJobTypeTask\"";

    public const string FromJoins = """
        setup."FgsJobTypeTask" t
        INNER JOIN setup."FgsJobTypeCategory" jtc
            ON jtc."Id" = t."JobTypeCategoryId"
           AND jtc."TenantId" = t."TenantId"
           AND jtc."CompanyId" = t."CompanyId"
        INNER JOIN setup."FgsJobCategory" jc
            ON jc."Id" = jtc."JobCategoryId"
           AND jc."TenantId" = t."TenantId"
           AND jc."CompanyId" = t."CompanyId"
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
        jc."Name" AS "CategoryName", tr."Name" AS "TradeName", sl."Name" AS "SkillName"
        """;

    public const string SelectSummaryColumns = """
        t."Id", t."JobTypeCategoryId", t."TradeId", t."SkillLevelId", t."Name", t."TaskName",
        t."Priority", t."EstimatedHours", t."DisplayOrder", t."IsActive",
        jc."Name" AS "CategoryName", tr."Name" AS "TradeName", sl."Name" AS "SkillName"
        """;

    public const string SelectLookupColumns = """
        "Id"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "JobTypeCategoryId", "TradeId", "SkillLevelId",
        "Name", "TaskName", "Priority", "EstimatedHours"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns, tableAlias: "t");
}
