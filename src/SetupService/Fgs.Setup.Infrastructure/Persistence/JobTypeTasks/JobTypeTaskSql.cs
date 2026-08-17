using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeTasks;

internal static class JobTypeTaskSql
{
    public const string Table = "setup.\"FgsJobTypeTask\"";

    public const string SelectDetailColumns = """
        "Id", "JobTypeCategoryId", "TradeId", "TaskName", "Priority", "EstimatedHours", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "JobTypeCategoryId", "TradeId", "TaskName", "Priority", "EstimatedHours", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "JobTypeCategoryId", "TradeId", "TaskName", "Priority", "EstimatedHours"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}
