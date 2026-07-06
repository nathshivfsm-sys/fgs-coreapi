using Fgs.Foundation.Paging;
namespace Fgs.Asset.Infrastructure.AssetModels;
internal static class FgsAssetModelSql
{
    public const string Table = "asset.\"FgsAssetModel\"";
    public const string SelectDetailColumns = "\"Id\", \"AssetTypeId\", \"AssetManufacturerId\", \"ModelNumber\", \"ModelDescription\", \"IsActive\"";
    public const string SelectSummaryColumns = SelectDetailColumns;
    public const string SelectLookupColumns = "\"Id\", \"ModelNumber\", \"ModelDescription\"";
    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase) { "Id", "AssetTypeId", "AssetManufacturerId", "ModelNumber", "ModelDescription", "IsActive" };
    public static string ResolveOrderBy(string? sortBy, SortDirection direction) { var dir = direction == SortDirection.Desc ? "DESC" : "ASC"; if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy)) return $"ORDER BY \"Id\" {dir}"; var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase)); return $"ORDER BY \"{column}\" {dir}"; }
}
