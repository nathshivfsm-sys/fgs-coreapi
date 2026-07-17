using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;

public interface IFgsUniversalMatrixSizeTierReadRepository
{
    Task<FgsUniversalMatrixSizeTierDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsUniversalMatrixSizeTierSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsUniversalMatrixSizeTierListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsUniversalMatrixSizeTierLookupDto>> LookupAsync(
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
