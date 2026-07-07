using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Entities.UniversalPricingMatrix.UniversalPricingServices;

internal static class FgsUniversalPricingServiceSql
{
    public const string Table = "setup.\"FgsUniversalPricingService\"";

    public const string SelectDetailColumns = """
        "Id", "UniversalPricingServiceCode", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "UniversalPricingServiceCode", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "UniversalPricingServiceCode", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "UniversalPricingServiceCode"
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
