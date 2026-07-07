using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Entities.UniversalPricingMatrix.UniversalMatrixItems;

internal static class FgsUniversalMatrixItemSql
{
    public const string Table = "setup.\"FgsUniversalMatrixItem\"";

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
        "Id", "IsActive", "DisplayOrder", "UniversalPricingServiceId", "ItemName", "UnitType", "BasePrice"
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
