using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SetupTaxAuthorities;

internal static class FgsSetupTaxAuthoritySql
{
    public const string Table = "setup.\"FgsSetupTaxAuthority\"";

    public const string SelectDetailColumns = """
        "Id", "Code", "Name", "RegionCode", "IsExternalSystemRecord", "TaxPercent", "Description", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Code", "Name", "RegionCode", "IsExternalSystemRecord", "TaxPercent", "Description", "IsActive"
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
            return $"ORDER BY \"Name\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"Name\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
