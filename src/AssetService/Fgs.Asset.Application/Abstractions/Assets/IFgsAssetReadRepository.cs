using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.Assets.Dtos;
using Fgs.Foundation.Paging;
namespace Fgs.Asset.Application.Abstractions.Assets;
public interface IFgsAssetReadRepository {
    Task<FgsAssetDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsAssetSummaryDto>> ListAsync(AssetListQuery query, FgsAssetListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsAssetLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsByAssetNumberAsync(string assetNumber, long? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetTypeIdAsync(long? assetTypeId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetManufacturerIdAsync(long? assetManufacturerId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetModelIdAsync(long? assetModelId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetStatusIdAsync(long assetStatusId, CancellationToken cancellationToken = default);
    Task<bool> ExistsServiceLocationIdAsync(long serviceLocationId, CancellationToken cancellationToken = default);
}
