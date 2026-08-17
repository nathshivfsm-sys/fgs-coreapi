using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.PriceBooks;

internal static class FgsPriceBookSql
{
    public const string Table = "setup.\"FgsPriceBook\"";
    public const string JobTypeTable = "setup.\"FgsJobType\"";

    public const string SelectDetailColumns = """
        "Id", "PriceBookCode", "PriceBookName", "Description", "JobTypeId", "PricingModel",
        "EstimatedDurationMinutes", "BasePrice", "IsTaxable", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "PriceBookCode", "PriceBookName", "JobTypeId", "PricingModel",
        "EstimatedDurationMinutes", "BasePrice", "IsTaxable", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "PriceBookCode", "PriceBookName", "PricingModel"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "PriceBookCode", "PriceBookName", "JobTypeId", "PricingModel",
        "EstimatedDurationMinutes", "BasePrice", "IsTaxable"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "PriceBookName");
}
