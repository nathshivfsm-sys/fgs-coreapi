using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SetupTaxes;

internal static class FgsSetupTaxSql
{
    public const string Table = "setup.\"FgsSetupTax\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "TaxCode", "Name", "IsExternalSystemRecord", "ExternalSystemId", "SyncToken", "ShowTaxDetail", "Description", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "TaxCode", "Name", "IsExternalSystemRecord", "ExternalSystemId", "SyncToken", "ShowTaxDetail", "Description", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "TaxCode", "Name"
        """;

    public const string TaxDetailTable = "setup.\"FgsSetupTaxDetail\"";

    public const string TaxAuthorityTable = "setup.\"FgsSetupTaxAuthority\"";

    public const string SelectTaxAssignmentColumns = """
        td."Id",
        td."FgsSetupTaxAuthorityId",
        ta."Code" AS "TaxAuthorityCode",
        ta."Name" AS "TaxAuthorityName",
        ta."TaxPercent",
        td."EffectiveFromDate",
        td."EffectiveToDate",
        td."IsExternalSystemRecord",
        td."IsActive"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "TaxCode", "Name", "IsExternalSystemRecord", "ExternalSystemId", "SyncToken", "ShowTaxDetail", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Name\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"Name\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
