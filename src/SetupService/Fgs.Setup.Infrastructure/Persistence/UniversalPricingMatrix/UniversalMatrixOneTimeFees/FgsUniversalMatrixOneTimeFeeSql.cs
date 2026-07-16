using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixOneTimeFees;

internal static class FgsUniversalMatrixOneTimeFeeSql
{
    public const string Table = "setup.\"FgsUniversalMatrixOneTimeFee\"";

    public const string SelectDetailColumns = """
        "Id", "UniversalPricingServiceId", "Name", "Amount", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "UniversalPricingServiceId", "Name", "Amount", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "UniversalPricingServiceId", "Name", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "UniversalPricingServiceId", "Name", "Amount"
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
