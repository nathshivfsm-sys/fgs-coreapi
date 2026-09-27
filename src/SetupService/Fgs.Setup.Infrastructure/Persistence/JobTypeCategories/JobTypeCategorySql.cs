using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeCategories;

internal static class JobTypeCategorySql
{
    public const string Table = "setup.\"FgsJobTypeCategory\"";

    public const string FromJoins = """
        setup."FgsJobTypeCategory" jtc
        INNER JOIN setup."FgsJobTypeTask" t
            ON t."Id" = jtc."JobTypeTaskId"
           AND t."TenantId" = jtc."TenantId"
           AND t."CompanyId" = jtc."CompanyId"
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
        jtc."Id", jtc."JobTypeId", jtc."JobTypeTaskId", jtc."DisplayOrder", jtc."IsActive",
        t."Name", t."TaskName", t."Priority", t."EstimatedHours",
        tr."Name" AS "TradeName", sl."Name" AS "SkillName"
        """;

    public const string SelectSummaryColumns = """
        jtc."Id", jtc."JobTypeId", jtc."JobTypeTaskId", jtc."DisplayOrder", jtc."IsActive",
        t."Name", t."TaskName", t."Priority", t."EstimatedHours",
        tr."Name" AS "TradeName", sl."Name" AS "SkillName"
        """;

    public const string SelectLookupColumns = """
        jtc."Id", jtc."JobTypeId", jtc."JobTypeTaskId", jtc."DisplayOrder",
        t."Name", t."TaskName", t."Priority", t."EstimatedHours",
        tr."Name" AS "TradeName", sl."Name" AS "SkillName"
        """;

    public const string SelectJobTypeChildColumns = """
        jtc."Id", jtc."JobTypeTaskId", jtc."DisplayOrder", jtc."IsActive",
        t."Name", t."TaskName", t."Priority", t."EstimatedHours",
        tr."Name" AS "TradeName", sl."Name" AS "SkillName"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "JobTypeId", "JobTypeTaskId"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns, tableAlias: "jtc");

}
