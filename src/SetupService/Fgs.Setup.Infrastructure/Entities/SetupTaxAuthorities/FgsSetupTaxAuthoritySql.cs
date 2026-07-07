using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Entities.SetupTaxAuthorities;

internal static class FgsSetupTaxAuthoritySql
{
    public const string Table = "setup.\"FgsSetupTaxAuthority\"";

    public const string TaxDetailTable = "setup.\"FgsSetupTaxDetail\"";

    private const string UsageCountColumn = """
        (SELECT COUNT(*)::integer
         FROM setup."FgsSetupTaxDetail" td
         WHERE td."FgsSetupTaxAuthorityId" = ta."Id"
           AND td."TenantId" = ta."TenantId"
           AND td."CompanyId" = ta."CompanyId"
           AND td."IsActive" = TRUE) AS "UsageCount"
        """;

    public const string SelectDetailColumns = $"""
        ta."Id", ta."Code", ta."Name", ta."RegionCode", ta."IsExternalSystemRecord", ta."TaxPercent", ta."Description", ta."IsActive",
        {UsageCountColumn}
        """;

    public const string SelectSummaryColumns = $"""
        ta."Id", ta."Code", ta."Name", ta."RegionCode", ta."IsExternalSystemRecord", ta."TaxPercent", ta."Description", ta."IsActive",
        {UsageCountColumn}
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name", "TaxPercent"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "Code", "Name", "RegionCode", "IsExternalSystemRecord", "TaxPercent", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY ta.\"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY ta.\"Name\" {dir}"
            : $"ORDER BY ta.\"{column}\" {dir}";
    }
}