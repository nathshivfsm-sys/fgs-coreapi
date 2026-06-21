using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SetupTaxAuthorities;

internal static class FgsSetupTaxAuthoritySql
{
    public const string Table = "setup.\"FgsSetupTaxAuthority\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "Code", "Name", "RegionCode", "IsExternalSystemRecord", "Description", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "Code", "Name", "RegionCode", "IsExternalSystemRecord", "Description", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "Code", "Name", "RegionCode", "IsExternalSystemRecord", "Description"
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
