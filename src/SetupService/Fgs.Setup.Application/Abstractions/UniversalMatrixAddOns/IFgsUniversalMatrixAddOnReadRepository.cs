using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;

public interface IFgsUniversalMatrixAddOnReadRepository
{
    Task<FgsUniversalMatrixAddOnDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsUniversalMatrixAddOnSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsUniversalMatrixAddOnListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsUniversalMatrixAddOnLookupDto>> LookupAsync(
        bool activeOnly = true,
        long? universalPricingServiceId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsUniversalPricingServiceIdAsync(
        long id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        long universalPricingServiceId,
        string name,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
