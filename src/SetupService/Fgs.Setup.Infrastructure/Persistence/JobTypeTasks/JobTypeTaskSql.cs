using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeTasks;

internal static class JobTypeTaskSql
{
    public const string Table = "setup.\"FgsJobTypeTask\"";

    public const string FromJoins = """
        setup."FgsJobTypeTask" t
        LEFT JOIN setup."FgsJobCategory" jc
            ON jc."Id" = t."JobCategoryId"
           AND jc."TenantId" = t."TenantId"
           AND jc."CompanyId" = t."CompanyId"
        """;

    public const string SelectDetailColumns = """
        t."Id", t."JobCategoryId", t."TradeId", t."SkillLevelId", t."Name", t."TaskName",
        t."Priority", t."EstimatedHours", t."DisplayOrder", t."IsActive",
        jc."Name" AS "CategoryName"
        """;

    public const string SelectSummaryColumns = """
        t."Id", t."JobCategoryId", t."TradeId", t."SkillLevelId", t."Name", t."TaskName",
        t."Priority", t."EstimatedHours", t."DisplayOrder", t."IsActive",
        jc."Name" AS "CategoryName"
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
        "Id", "IsActive", "DisplayOrder", "JobCategoryId", "TradeId", "SkillLevelId",
        "Name", "TaskName", "Priority", "EstimatedHours"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns, tableAlias: "t");
}
