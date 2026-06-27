using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Warehouses;

internal static class FgsWarehouseSql
{
    public const string Table = "setup.\"FgsWarehouse\"";

    public const string SelectDetailColumns = """
        "Id", "WarehouseCode", "Name", "WarehouseType", "AddressId", "Description", "IsDefault", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "WarehouseCode", "Name", "WarehouseType", "AddressId", "Description", "IsDefault", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "WarehouseCode", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "WarehouseCode", "Name", "WarehouseType", "AddressId", "Description", "IsDefault"
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
