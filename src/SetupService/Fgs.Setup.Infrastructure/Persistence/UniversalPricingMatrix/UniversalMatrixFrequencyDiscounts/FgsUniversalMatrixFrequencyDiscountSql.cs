using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixFrequencyDiscounts;

internal static class FgsUniversalMatrixFrequencyDiscountSql
{
    public const string Table = "setup.\"FgsUniversalMatrixFrequencyDiscount\"";
    public const string ParentTable = "setup.\"FgsUniversalPricingService\"";

    public const string SelectDetailColumns = """
        "Id", "UniversalPricingServiceId", "Name", "DiscountPercent", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "UniversalPricingServiceId", "Name", "DiscountPercent", "DisplayOrder", "IsActive"
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
        "DiscountPercent",
        "DisplayOrder"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"Id\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
