using Fgs.Foundation.Paging;

namespace Fgs.Inventory.Infrastructure.TruckStockTemplates;

internal static class FgsTruckStockTemplateSql
{
    public const string Table = "inventory.\"FgsTruckStockTemplate\"";

    public const string SelectDetailColumns = """
        "Id", "TemplateCode", "Name", "Description", "IsActive"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        "Id", "TemplateCode", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "TemplateCode", "Name", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
