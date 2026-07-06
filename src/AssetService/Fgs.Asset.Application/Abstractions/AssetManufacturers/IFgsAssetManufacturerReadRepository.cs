using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Foundation.Paging;

namespace Fgs.Asset.Application.Abstractions.AssetManufacturers;

public interface IFgsAssetManufacturerReadRepository
{
    Task<FgsAssetManufacturerDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsAssetManufacturerSummaryDto>> ListAsync(AssetListQuery query, FgsAssetManufacturerListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsAssetManufacturerLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, long? excludeId = null, CancellationToken cancellationToken = default);
}
