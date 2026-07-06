using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Foundation.Paging;
namespace Fgs.Asset.Application.Abstractions.AssetModels;
public interface IFgsAssetModelReadRepository
{
    Task<FgsAssetModelDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsAssetModelSummaryDto>> ListAsync(AssetListQuery query, FgsAssetModelListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsAssetModelLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetTypeIdAsync(long assetTypeId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetManufacturerIdAsync(long assetManufacturerId, CancellationToken cancellationToken = default);
}
