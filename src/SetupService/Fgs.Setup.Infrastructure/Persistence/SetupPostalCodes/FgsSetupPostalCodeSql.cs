using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPostalCodes;

internal static class FgsSetupPostalCodeSql
{
    public const string Table = "setup.\"FgsSetupPostalCode\"";

    public const string SelectDetailColumns = """
        "Id", "PostalCode", "CountryCode", "StateProvinceCode", "City", "TripChargeAmount",
        "FgsSetupZoneId", "FgsSetupTaxId", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "PostalCode", "CountryCode", "StateProvinceCode", "City", "TripChargeAmount",
        "FgsSetupZoneId", "FgsSetupTaxId", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "PostalCode"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "PostalCode", "CountryCode", "StateProvinceCode", "City",
        "TripChargeAmount", "FgsSetupZoneId", "FgsSetupTaxId"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
