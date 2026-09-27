using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypes;

internal static class JobTypeSql
{
    public const string Table = "setup.\"FgsJobType\"";

    public const string SelectDetailColumns = """
        "Id", "JobTypeCode", "Name", "UsedFor", "BusinessUnit", "ShowToFieldTech", "ShowOnCustomerPortal", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "JobTypeCode", "Name", "UsedFor", "BusinessUnit", "ShowToFieldTech", "ShowOnCustomerPortal", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "JobTypeCode", "Name", "DisplayOrder"
        """;

    public const string RelatedTaskNameExists = """
        EXISTS (
            SELECT 1
            FROM setup."FgsJobTypeCategory" jtc
            INNER JOIN setup."FgsJobTypeTask" t
                ON t."Id" = jtc."JobTypeTaskId"
               AND t."TenantId" = jtc."TenantId"
               AND t."CompanyId" = jtc."CompanyId"
            WHERE jtc."JobTypeId" = jt."Id"
              AND jtc."TenantId" = @TenantId
              AND jtc."CompanyId" = @CompanyId
              AND t."TaskName" ILIKE @Search
        )
        """;

    public const string RelatedJobTypeTaskIdExists = """
        EXISTS (
            SELECT 1
            FROM setup."FgsJobTypeCategory" jtc
            WHERE jtc."JobTypeId" = jt."Id"
              AND jtc."JobTypeTaskId" = @JobTypeTaskId
              AND jtc."TenantId" = @TenantId
              AND jtc."CompanyId" = @CompanyId
        )
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "JobTypeCode", "Name", "UsedFor", "BusinessUnit", "ShowToFieldTech", "ShowOnCustomerPortal"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}
