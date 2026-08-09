using Fgs.Foundation.Paging;

namespace Fgs.Asset.Infrastructure.Assets;

internal static class FgsAssetSql
{
    public const string Table = "asset.\"FgsAsset\"";

    public const string SelectDetailColumns =
        "\"Id\", \"AssetGuid\", \"AssetNumber\", \"ServiceLocationId\", \"UnitNumber\", \"AssetTypeId\", \"AssetManufacturerId\", \"AssetModelId\", \"AssetDescription\", \"CustomerAssetNumber\", \"CustomerAssetName\", \"ManufacturerName\", \"ModelNumber\", \"SerialNumber\", \"ManufactureDate\", \"InstallDate\", \"InstalledWorkOrderId\", \"IsInstalledByCompany\", \"IsOurInstallation\", \"AssetStatusId\", \"IsActive\"";

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = "\"Id\", \"AssetNumber\", \"CustomerAssetName\"";

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "AssetNumber", "ServiceLocationId", "AssetStatusId", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        return $"ORDER BY \"{AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase))}\" {dir}";
    }
}
