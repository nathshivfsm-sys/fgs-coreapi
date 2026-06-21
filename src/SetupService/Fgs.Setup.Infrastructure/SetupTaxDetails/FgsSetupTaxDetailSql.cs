using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SetupTaxDetails;

internal static class FgsSetupTaxDetailSql
{
    public const string Table = "setup.\"FgsSetupTaxDetail\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "FgsSetupTaxId", "FgsSetupTaxAuthorityId", "EffectiveFromDate", "EffectiveToDate", "TaxPercent", "IsExternalSystemRecord", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "FgsSetupTaxId", "FgsSetupTaxAuthorityId", "EffectiveFromDate", "EffectiveToDate", "TaxPercent", "IsExternalSystemRecord", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "FgsSetupTaxId", "FgsSetupTaxAuthorityId", "EffectiveFromDate", "TaxPercent"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "FgsSetupTaxId", "FgsSetupTaxAuthorityId", "EffectiveFromDate", "EffectiveToDate", "TaxPercent", "IsExternalSystemRecord"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"EffectiveFromDate\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"EffectiveFromDate\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
