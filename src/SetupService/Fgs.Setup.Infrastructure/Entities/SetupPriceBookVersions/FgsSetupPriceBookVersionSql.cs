using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Entities.SetupPriceBookVersions;

internal static class FgsSetupPriceBookVersionSql
{
    public const string Table = "setup.\"FgsSetupPriceBookVersion\"";

    public const string SelectDetailColumns = """
        "Id", "Version", "EffectiveFrom", "EffectiveTo", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Version", "EffectiveFrom", "EffectiveTo", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Version", "EffectiveFrom", "EffectiveTo"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "Version", "EffectiveFrom", "EffectiveTo", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "EffectiveFrom");
}
