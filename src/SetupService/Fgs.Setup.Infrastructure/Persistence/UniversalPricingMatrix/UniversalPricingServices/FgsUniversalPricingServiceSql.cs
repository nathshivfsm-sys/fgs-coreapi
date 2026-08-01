using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalPricingServices;

internal static class FgsUniversalPricingServiceSql
{
    public const string Table = "setup.\"FgsUniversalPricingService\"";
    public const string TierTable = "setup.\"FgsUniversalMatrixTier\"";
    public const string SizeTierTable = "setup.\"FgsUniversalMatrixSizeTier\"";
    public const string ItemTable = "setup.\"FgsUniversalMatrixItem\"";
    public const string FrequencyDiscountTable = "setup.\"FgsUniversalMatrixFrequencyDiscount\"";
    public const string OneTimeFeeTable = "setup.\"FgsUniversalMatrixOneTimeFee\"";
    public const string AddOnTable = "setup.\"FgsUniversalMatrixAddOn\"";

    public const string SelectDetailColumns = """
        "Id", "UniversalPricingServiceCode", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "UniversalPricingServiceCode", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "UniversalPricingServiceCode", "DisplayOrder"
        """;

    public const string SelectTierColumns = """
        "Id", "Name", "Multiplier", "DisplayOrder", "IsActive"
        """;

    public const string SelectSizeTierColumns = """
        "Id", "Name", "Multiplier", "DisplayOrder", "IsActive"
        """;

    public const string SelectItemColumns = """
        "Id", "ItemName", "UnitType", "BasePrice", "DisplayOrder", "IsActive"
        """;

    public const string SelectFrequencyDiscountColumns = """
        "Id", "Name", "DiscountPercent", "DisplayOrder", "IsActive"
        """;

    public const string SelectOneTimeFeeColumns = """
        "Id", "Name", "Amount", "DisplayOrder", "IsActive"
        """;

    public const string SelectAddOnColumns = """
        "Id", "Name", "UnitType", "Price", "DisplayOrder", "IsActive"
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
