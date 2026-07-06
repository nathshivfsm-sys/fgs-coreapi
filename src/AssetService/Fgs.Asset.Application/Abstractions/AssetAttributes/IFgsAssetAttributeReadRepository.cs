using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Foundation.Paging;
namespace Fgs.Asset.Application.Abstractions.AssetAttributes;
public interface IFgsAssetAttributeReadRepository {
    Task<FgsAssetAttributeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsAssetAttributeSummaryDto>> ListAsync(AssetListQuery query, FgsAssetAttributeListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsAssetAttributeLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsByAttributeCodeAsync(long assetTypeId, string attributeCode, long? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetTypeIdAsync(long assetTypeId, CancellationToken cancellationToken = default);
}
