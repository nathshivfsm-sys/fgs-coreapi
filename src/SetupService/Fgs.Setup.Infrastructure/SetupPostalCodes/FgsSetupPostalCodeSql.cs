using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SetupPostalCodes;

internal static class FgsSetupPostalCodeSql
{
    public const string Table = "setup.\"FgsSetupPostalCode\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "PostalCode", "FgsSetupZoneId", "FgsSetupTaxId", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "PostalCode", "FgsSetupZoneId", "FgsSetupTaxId", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "PostalCode"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "PostalCode", "FgsSetupZoneId", "FgsSetupTaxId"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"PostalCode\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"PostalCode\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
