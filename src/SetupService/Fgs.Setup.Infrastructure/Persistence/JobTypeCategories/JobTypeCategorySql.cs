using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeCategories;

internal static class JobTypeCategorySql
{
    public const string Table = "setup.\"FgsJobTypeCategory\"";

    public const string SelectDetailColumns = """
        "Id", "JobTypeId", "JobTypeTaskId", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "JobTypeId", "JobTypeTaskId", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "JobTypeId", "JobTypeTaskId", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "JobTypeId", "JobTypeTaskId"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}
