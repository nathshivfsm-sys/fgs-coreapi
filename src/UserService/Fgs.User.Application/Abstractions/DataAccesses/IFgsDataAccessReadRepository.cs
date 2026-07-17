using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.DataAccesses.Dtos;

namespace Fgs.User.Application.Abstractions.DataAccesses;

public interface IFgsDataAccessReadRepository
{
    Task<FgsDataAccessDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsDataAccessSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsDataAccessListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsDataAccessLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByDataAccessCodeAsync(
        string dataAccessCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
