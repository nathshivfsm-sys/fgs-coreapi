using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPricingMatrixMaterialTiers;

public interface IFgsSetupPricingMatrixMaterialTierReadRepository
{
    Task<FgsSetupPricingMatrixMaterialTierDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsSetupPricingMatrixMaterialTierSummaryDto>> ListAsync(SetupListQuery query, FgsSetupPricingMatrixMaterialTierListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsSetupPricingMatrixMaterialTierLookupDto>> LookupAsync(bool activeOnly = true, long? pricingMatrixId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsPricingMatrixIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByFromCostAsync(long matrixId, decimal fromCost, long? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsActiveOtherItemsForMatrixAsync(long matrixId, CancellationToken cancellationToken = default);
}
