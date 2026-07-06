using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Foundation.Paging;
namespace Fgs.Asset.Application.Abstractions.AssetWarranties;
public interface IFgsAssetWarrantyReadRepository {
    Task<FgsAssetWarrantyDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsAssetWarrantySummaryDto>> ListAsync(AssetListQuery query, FgsAssetWarrantyListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsAssetWarrantyLookupDto>> LookupAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetIdAsync(long assetId, CancellationToken cancellationToken = default);
}
