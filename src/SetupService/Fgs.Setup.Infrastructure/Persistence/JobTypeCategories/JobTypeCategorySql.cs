using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypeCategories;

internal static class JobTypeCategorySql
{
    public const string Table = "setup.\"FgsJobTypeCategory\"";

    public const string SelectDetailColumns = """
        "Id", "JobTypeId", "JobCategoryId", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "JobTypeId", "JobCategoryId", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "JobTypeId", "JobCategoryId", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "JobTypeId", "JobCategoryId"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}
