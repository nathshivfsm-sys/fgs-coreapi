using Fgs.Foundation.Paging;
namespace Fgs.Asset.Infrastructure.AssetAttributes;
internal static class FgsAssetAttributeSql {
    public const string Table = "asset.\"FgsAssetAttribute\"";
    public const string SelectDetailColumns = "\"Id\", \"AssetTypeId\", \"AttributeCode\", \"AttributeName\", \"InputType\", \"DefaultOptionId\", \"DefaultValueText\", \"DefaultValueInteger\", \"DefaultValueDecimal\", \"DefaultValueDate\", \"DefaultValueBoolean\", \"IsRequired\", \"IsSearchable\", \"DisplayOrder\", \"IsActive\"";
    public const string SelectSummaryColumns = SelectDetailColumns;
    public const string SelectLookupColumns = "\"Id\", \"AttributeCode\", \"AttributeName\"";
    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase) { "Id", "AssetTypeId", "AttributeCode", "AttributeName", "InputType", "DisplayOrder", "IsActive" };
    public static string ResolveOrderBy(string? sortBy, SortDirection direction) { var dir = direction == SortDirection.Desc ? "DESC" : "ASC"; if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy)) return $"ORDER BY \"DisplayOrder\" {dir}, \"Id\" {dir}"; return $"ORDER BY \"{AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase))}\" {dir}"; }
}
