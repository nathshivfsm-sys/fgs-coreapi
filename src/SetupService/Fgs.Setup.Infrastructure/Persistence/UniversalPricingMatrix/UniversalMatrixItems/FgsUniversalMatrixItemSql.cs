using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixItems;

internal static class FgsUniversalMatrixItemSql
{
    public const string Table = "setup.\"FgsUniversalMatrixItem\"";
    public const string ParentTable = "setup.\"FgsUniversalPricingService\"";

    public const string SelectDetailColumns = """
        "Id", "UniversalPricingServiceId", "ItemName", "UnitType", "BasePrice", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "UniversalPricingServiceId", "ItemName", "UnitType", "BasePrice", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "UniversalPricingServiceId", "ItemName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id",
        "IsActive",
        "UniversalPricingServiceId",
        "ItemName",
        "UnitType",
        "BasePrice",
        "DisplayOrder"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}
