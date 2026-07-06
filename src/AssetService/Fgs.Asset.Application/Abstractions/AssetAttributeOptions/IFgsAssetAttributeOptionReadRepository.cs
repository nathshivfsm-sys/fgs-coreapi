using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Foundation.Paging;
namespace Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
public interface IFgsAssetAttributeOptionReadRepository {
    Task<FgsAssetAttributeOptionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsAssetAttributeOptionSummaryDto>> ListAsync(AssetListQuery query, FgsAssetAttributeOptionListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsAssetAttributeOptionLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsByOptionCodeAsync(long assetAttributeId, string optionCode, long? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetAttributeIdAsync(long assetAttributeId, CancellationToken cancellationToken = default);
}
