using Fgs.Foundation.Paging;

namespace Fgs.Asset.Infrastructure.AssetTypes;

internal static class FgsAssetTypeSql
{
    public const string Table = "asset.\"FgsAssetType\"";
    public const string SelectDetailColumns = "\"Id\", \"Code\", \"Name\", \"Description\", \"IsActive\"";
    public const string SelectSummaryColumns = SelectDetailColumns;
    public const string SelectLookupColumns = "\"Id\", \"Code\", \"Name\"";
    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase) { "Id", "IsActive", "Code", "Name", "Description" };
    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy)) return $"ORDER BY \"Id\" {dir}";
        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
