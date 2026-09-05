using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupTaxes;

internal static class FgsSetupTaxSql
{
    public const string Table = "setup.\"FgsSetupTax\"";

    public const string TaxDetailTable = "setup.\"FgsSetupTaxDetail\"";

    public const string TaxAuthorityTable = "setup.\"FgsSetupTaxAuthority\"";

    private const string TaxRateColumn = """
        COALESCE((
            SELECT SUM(ta."TaxPercent")
            FROM setup."FgsSetupTaxDetail" td
            INNER JOIN setup."FgsSetupTaxAuthority" ta
                ON ta."Id" = td."FgsSetupTaxAuthorityId"
               AND ta."TenantId" = td."TenantId"
               AND ta."CompanyId" = td."CompanyId"
            WHERE td."FgsSetupTaxId" = t."Id"
              AND td."TenantId" = t."TenantId"
              AND td."CompanyId" = t."CompanyId"
              AND td."IsActive" = TRUE
        ), 0) AS "TaxRate"
        """;

    public const string SelectTaxDetailColumns = """
        td."Id", td."FgsSetupTaxAuthorityId", ta."Code" AS "TaxAuthorityCode", ta."Name" AS "TaxAuthorityName",
        ta."TaxPercent", td."EffectiveFromDate", td."EffectiveToDate", td."IsExternalSystemRecord", td."IsActive"
        """;

    public const string SelectDetailColumns = """
        t."Id", t."TaxCode", t."Name", t."IsExternalSystemRecord", t."ExternalSystemId", t."SyncToken", t."ShowTaxDetail", t."Description", t."IsActive"
        """;

    public const string SelectSummaryColumns = $"""
        t."Id", t."TaxCode", t."Name", t."IsExternalSystemRecord", t."ExternalSystemId", t."SyncToken", t."ShowTaxDetail", t."Description", t."IsActive",
        {TaxRateColumn}
        """;

    public const string SelectLookupColumns = $"""
        t."Id", t."TaxCode", t."Name",
        {TaxRateColumn}
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "TaxCode", "Name", "IsExternalSystemRecord", "ExternalSystemId", "SyncToken", "ShowTaxDetail", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            tableAlias: "t");

}