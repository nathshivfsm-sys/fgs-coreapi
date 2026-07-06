using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Foundation.Paging;

namespace Fgs.Asset.Application.Abstractions.AssetTypes;

public interface IFgsAssetTypeReadRepository
{
    Task<FgsAssetTypeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsAssetTypeSummaryDto>> ListAsync(AssetListQuery query, FgsAssetTypeListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsAssetTypeLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, long? excludeId = null, CancellationToken cancellationToken = default);
}
