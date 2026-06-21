using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.Warehouses;

internal static class FgsWarehouseSql
{
    public const string Table = "setup.\"FgsWarehouse\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "WarehouseCode", "Name", "WarehouseType", "AddressId", "Description", "IsDefault", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "WarehouseCode", "Name", "WarehouseType", "AddressId", "Description", "IsDefault", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "WarehouseCode", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "WarehouseCode", "Name", "WarehouseType", "AddressId", "Description", "IsDefault"
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
