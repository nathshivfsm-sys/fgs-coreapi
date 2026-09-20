using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPostalCodes;

internal static class FgsSetupPostalCodeSql
{
    public const string Table = "setup.\"FgsSetupPostalCode\"";
    public const string ZoneTable = "setup.\"FgsSetupZone\"";
    public const string TaxTable = "setup.\"FgsSetupTax\"";

    public const string SelectDetailColumns = """
        pc."Id", pc."PostalCode", pc."CountryCode", pc."StateProvinceCode", pc."City", pc."TripChargeAmount",
        pc."FgsSetupZoneId", z."Name" AS "ZoneName", pc."FgsSetupTaxId", tax."Name" AS "TaxName", pc."IsActive"
        """;

    public const string SelectSummaryColumns = """
        pc."Id", pc."PostalCode", pc."CountryCode", pc."StateProvinceCode", pc."City", pc."TripChargeAmount",
        pc."FgsSetupZoneId", z."Name" AS "ZoneName", pc."FgsSetupTaxId", tax."Name" AS "TaxName", pc."IsActive"
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
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            tableAlias: "pc");
}
