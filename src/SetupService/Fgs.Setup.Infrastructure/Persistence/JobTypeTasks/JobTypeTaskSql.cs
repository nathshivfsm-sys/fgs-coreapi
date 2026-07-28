using Fgs.Foundation.Paging;

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
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"Id\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
