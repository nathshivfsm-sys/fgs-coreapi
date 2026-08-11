using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixTiers;

internal static class FgsUniversalMatrixTierSql
{
    public const string Table = "setup.\"FgsUniversalMatrixTier\"";
    public const string ParentTable = "setup.\"FgsUniversalPricingService\"";

    public const string SelectDetailColumns = """
        "Id", "UniversalPricingServiceId", "Name", "Multiplier", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "UniversalPricingServiceId", "Name", "Multiplier", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "UniversalPricingServiceId", "Name", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id",
        "IsActive",
        "UniversalPricingServiceId",
        "Name",
        "Multiplier",
        "DisplayOrder"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}
