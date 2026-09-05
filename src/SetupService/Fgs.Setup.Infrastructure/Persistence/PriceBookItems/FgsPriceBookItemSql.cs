using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.PriceBookItems;

internal static class FgsPriceBookItemSql
{
    public const string Table = "setup.\"FgsPriceBookItem\"";
    public const string ParentTable = "setup.\"FgsPriceBook\"";

    public const string SelectDetailColumns = """
        "Id", "PriceBookId", "InventoryItemId", "ItemCode", "ItemDescription", "Quantity", "DisplayOrder", "Notes"
        """;

    public const string SelectSummaryColumns = """
        "Id", "PriceBookId", "InventoryItemId", "ItemCode", "ItemDescription", "Quantity", "DisplayOrder"
        """;

    public const string SelectLookupColumns = """
        "Id", "PriceBookId", "ItemCode", "ItemDescription", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "PriceBookId", "ItemCode", "ItemDescription", "Quantity", "DisplayOrder"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "DisplayOrder");
}
