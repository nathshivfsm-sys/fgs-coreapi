using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupZones;

internal static class FgsSetupZoneSql
{
    public const string Table = "setup.\"FgsSetupZone\"";

    public const string SelectDetailColumns = """
        "Id", "Code", "Name", "Description", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Code", "Name", "Description", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "Code", "Name", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "Name");

}