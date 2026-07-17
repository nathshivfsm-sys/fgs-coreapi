using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;

namespace Fgs.User.Application.Abstractions.DataAccessScopes;

public interface IFgsDataAccessScopeReadRepository
{
    Task<FgsDataAccessScopeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsDataAccessScopeSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsDataAccessScopeListFilters filters,
        CancellationToken cancellationToken = default);
}
