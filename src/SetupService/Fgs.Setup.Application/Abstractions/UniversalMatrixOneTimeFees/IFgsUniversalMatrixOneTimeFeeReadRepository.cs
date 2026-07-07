using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalMatrixOneTimeFees;

public interface IFgsUniversalMatrixOneTimeFeeReadRepository
{
    Task<FgsUniversalMatrixOneTimeFeeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsUniversalMatrixOneTimeFeeSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsUniversalMatrixOneTimeFeeListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsUniversalMatrixOneTimeFeeLookupDto>> LookupAsync(
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
