using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.JobCategories;

internal static class JobCategorySql
{
    public const string Table = "setup.\"FgsJobCategory\"";

    public const string SelectDetailColumns = """
        "Id", "CategoryCode", "Name", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "CategoryCode", "Name", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "CategoryCode", "Name", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "CategoryCode", "Name"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}
