using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixItems;

public interface IFgsUniversalMatrixItemReadRepository
{
    Task<FgsUniversalMatrixItemDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsUniversalMatrixItemSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsUniversalMatrixItemListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsUniversalMatrixItemLookupDto>> LookupAsync(
        bool activeOnly = true,
        long? universalPricingServiceId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByUniversalPricingServiceIdAndItemNameAsync(
        long universalPricingServiceId, string itemName,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsUniversalPricingServiceIdAsync(
        long id,
        CancellationToken cancellationToken = default);
}
