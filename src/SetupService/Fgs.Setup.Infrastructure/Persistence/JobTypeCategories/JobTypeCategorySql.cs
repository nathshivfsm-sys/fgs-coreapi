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
        LEFT JOIN setup."FgsJobCategory" jc
            ON jc."Id" = t."JobCategoryId"
           AND jc."TenantId" = t."TenantId"
           AND jc."CompanyId" = t."CompanyId"
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
        t."JobCategoryId" AS "CategoryId",
        jc."Name" AS "CategoryName",
        jtc."JobTypeTaskId",
        t."Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "JobTypeId", "JobTypeTaskId"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns, tableAlias: "jtc");

}
