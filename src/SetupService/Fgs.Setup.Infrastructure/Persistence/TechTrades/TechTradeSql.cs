using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.TechTrades;

internal static class TechTradeSql
{
    public const string Table = "setup.\"FgsSetupTechTrade\"";

    public const string SelectDetailColumns = """
        "Id", "TradeCode", "Name", "Description", "SortOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TradeCode", "Name", "SortOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "TradeCode", "Name", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "TradeCode", "Name", "SortOrder", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            nullsLastTiebreakerColumn: "Name");

}