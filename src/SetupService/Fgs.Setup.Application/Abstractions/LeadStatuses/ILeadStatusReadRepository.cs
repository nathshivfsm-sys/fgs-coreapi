using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;

namespace Fgs.Setup.Application.Abstractions.LeadStatuses;

public interface ILeadStatusReadRepository
{
    Task<LeadStatusDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<LeadStatusSummaryDto>> ListAsync(
        SetupListQuery query,
        LeadStatusListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LeadStatusLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByStatusCodeAsync(
        string statusCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByStatusNameAsync(
        string statusName,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
