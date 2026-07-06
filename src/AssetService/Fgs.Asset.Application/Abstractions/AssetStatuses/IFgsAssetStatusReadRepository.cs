using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Foundation.Paging;

namespace Fgs.Asset.Application.Abstractions.AssetStatuses;

public interface IFgsAssetStatusReadRepository
{
    Task<FgsAssetStatusDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsAssetStatusSummaryDto>> ListAsync(AssetListQuery query, FgsAssetStatusListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsAssetStatusLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, long? excludeId = null, CancellationToken cancellationToken = default);
}
