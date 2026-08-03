using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;

public interface IFgsSetupPricingMatrixOtherReadRepository
{
    Task<FgsSetupPricingMatrixOtherDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsSetupPricingMatrixOtherSummaryDto>> ListAsync(SetupListQuery query, FgsSetupPricingMatrixOtherListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsSetupPricingMatrixOtherLookupDto>> LookupAsync(bool activeOnly = true, long? pricingMatrixId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsPricingMatrixIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCategoryCodeAsync(long matrixId, string categoryCode, long? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsActiveMaterialTiersForMatrixAsync(long matrixId, CancellationToken cancellationToken = default);
}
