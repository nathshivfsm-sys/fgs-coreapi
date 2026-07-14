using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;

namespace Fgs.User.Application.Abstractions.RoleDataAccesses;

public interface IFgsRoleDataAccessReadRepository
{
    Task<FgsRoleDataAccessDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsRoleDataAccessSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsRoleDataAccessListFilters filters,
        CancellationToken cancellationToken = default);
}
