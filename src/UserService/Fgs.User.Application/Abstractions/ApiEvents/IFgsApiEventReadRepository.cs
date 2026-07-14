using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiEvents.Dtos;

namespace Fgs.User.Application.Abstractions.ApiEvents;

public interface IFgsApiEventReadRepository
{
    Task<FgsApiEventDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsApiEventSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsApiEventListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsApiEventLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEventCodeAsync(
        string eventCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
