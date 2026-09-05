using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.FgsBusinessTypes;

internal static class FgsBusinessTypeSql
{
    public const string Table = "setup.\"FgsBusinessType\"";

    public const string SelectDetailColumns = """
        "Id", "Code", "Name", "Description", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Code", "Name", "Description", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "Code", "Name", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            nullsLastTiebreakerColumn: "Name");

}