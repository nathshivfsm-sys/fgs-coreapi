using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.ResolutionCodes;

internal static class ResolutionCodeSql
{
    public const string Table = "setup.\"FgsResolutionCode\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "GloResolutionTypeId", "ResolutionCode", "ResolutionName", "IsMobileVisible", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "GloResolutionTypeId", "ResolutionCode", "ResolutionName", "IsMobileVisible", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "ResolutionCode", "ResolutionName"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "GloResolutionTypeId", "ResolutionCode", "ResolutionName", "IsMobileVisible"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"ResolutionName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"ResolutionName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
