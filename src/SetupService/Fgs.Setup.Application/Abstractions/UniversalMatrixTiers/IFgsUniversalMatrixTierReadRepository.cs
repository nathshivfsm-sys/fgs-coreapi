using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixTiers;

public interface IFgsUniversalMatrixTierReadRepository
{
    Task<FgsUniversalMatrixTierDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsUniversalMatrixTierSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsUniversalMatrixTierListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsUniversalMatrixTierLookupDto>> LookupAsync(
        bool activeOnly = true,
        long? universalPricingServiceId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByUniversalPricingServiceIdAndNameAsync(
        long universalPricingServiceId, string name,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsUniversalPricingServiceIdAsync(
        long id,
        CancellationToken cancellationToken = default);
}
