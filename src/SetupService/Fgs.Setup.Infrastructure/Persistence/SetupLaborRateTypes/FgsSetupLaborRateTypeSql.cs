using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupLaborRateTypes;

internal static class FgsSetupLaborRateTypeSql
{
    public const string Table = "setup.\"FgsSetupLaborRateType\"";

    public const string SelectDetailColumns = """
        "Id", "Name", "Description", "SortOrder", "IsSystem", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Name", "Description", "SortOrder", "IsSystem", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Name", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "SortOrder", "Name", "Description", "IsSystem"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "SortOrder", nullsLastTiebreakerColumn: "Name");

}