using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupTaxes;

public interface IFgsSetupTaxReadRepository
{
    Task<FgsSetupTaxDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupTaxSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupTaxListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupTaxLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByTaxCodeAsync(
        string taxCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
