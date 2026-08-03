using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;

public interface IFgsUniversalMatrixFrequencyDiscountReadRepository
{
    Task<FgsUniversalMatrixFrequencyDiscountDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsUniversalMatrixFrequencyDiscountSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsUniversalMatrixFrequencyDiscountListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsUniversalMatrixFrequencyDiscountLookupDto>> LookupAsync(
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
