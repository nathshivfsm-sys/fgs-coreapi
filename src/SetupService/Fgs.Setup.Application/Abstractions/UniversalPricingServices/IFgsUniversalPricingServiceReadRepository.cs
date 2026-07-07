using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;

namespace Fgs.Setup.Application.Abstractions.UniversalPricingServices;

public interface IFgsUniversalPricingServiceReadRepository
{
    Task<FgsUniversalPricingServiceDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsUniversalPricingServiceSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsUniversalPricingServiceListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsUniversalPricingServiceLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByUniversalPricingServiceCodeAsync(
        string universalPricingServiceCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsGloUniversalPricingServiceCodeAsync(
        string id,
        CancellationToken cancellationToken = default);
}
