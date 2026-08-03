using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPricingMatrixLabors;

public interface IFgsSetupPricingMatrixLaborReadRepository
{
    Task<FgsSetupPricingMatrixLaborDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<FgsSetupPricingMatrixLaborSummaryDto>> ListAsync(SetupListQuery query, FgsSetupPricingMatrixLaborListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FgsSetupPricingMatrixLaborLookupDto>> LookupAsync(bool activeOnly = true, long? pricingMatrixId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsPricingMatrixIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<long?> GetPricingMatrixIdAsync(long laborId, bool activeOnly = true, CancellationToken cancellationToken = default);
}
