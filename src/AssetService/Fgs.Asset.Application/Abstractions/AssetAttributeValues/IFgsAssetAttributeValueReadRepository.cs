using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using Fgs.Foundation.Paging;
namespace Fgs.Asset.Application.Abstractions.AssetAttributeValues;
public interface IFgsAssetAttributeValueReadRepository {
    Task<FgsAssetAttributeValueDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsAssetAttributeValueSummaryDto>> ListAsync(AssetListQuery query, FgsAssetAttributeValueListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsAssetAttributeValueLookupDto>> LookupAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetIdAsync(long assetId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetAttributeIdAsync(long assetAttributeId, CancellationToken cancellationToken = default);
}
