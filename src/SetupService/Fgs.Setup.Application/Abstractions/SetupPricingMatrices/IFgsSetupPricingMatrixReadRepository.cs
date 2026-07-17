using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupPricingMatrices;

public interface IFgsSetupPricingMatrixReadRepository
{
    Task<FgsSetupPricingMatrixDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupPricingMatrixSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupPricingMatrixListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupPricingMatrixLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
