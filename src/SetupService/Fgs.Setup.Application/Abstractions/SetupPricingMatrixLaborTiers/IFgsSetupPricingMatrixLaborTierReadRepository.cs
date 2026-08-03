using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPricingMatrixLaborTiers;

public interface IFgsSetupPricingMatrixLaborTierReadRepository
{
    Task<FgsSetupPricingMatrixLaborTierDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsSetupPricingMatrixLaborTierSummaryDto>> ListAsync(SetupListQuery query, FgsSetupPricingMatrixLaborTierListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsSetupPricingMatrixLaborTierLookupDto>> LookupAsync(bool activeOnly = true, long? pricingMatrixLaborId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsPricingMatrixLaborIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySequenceOrderAsync(long laborId, short sequenceOrder, long? excludeId = null, CancellationToken cancellationToken = default);
}
