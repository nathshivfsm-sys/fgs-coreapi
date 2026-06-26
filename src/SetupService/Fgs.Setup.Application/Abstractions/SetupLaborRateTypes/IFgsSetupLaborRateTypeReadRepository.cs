using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;

namespace Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;

public interface IFgsSetupLaborRateTypeReadRepository
{
    Task<FgsSetupLaborRateTypeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsSetupLaborRateTypeSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupLaborRateTypeListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsSetupLaborRateTypeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        string name,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
