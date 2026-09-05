using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;

namespace Fgs.Setup.Application.Abstractions.NonWorkingDates;

public interface IFgsNonWorkingDateReadRepository
{
    Task<FgsNonWorkingDateDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsNonWorkingDateSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsNonWorkingDateListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsNonWorkingDateLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNonWorkingDateAsync(
        DateOnly nonWorkingDate,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
