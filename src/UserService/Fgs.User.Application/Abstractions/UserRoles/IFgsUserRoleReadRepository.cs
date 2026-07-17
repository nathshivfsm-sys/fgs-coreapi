using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.UserRoles.Dtos;

namespace Fgs.User.Application.Abstractions.UserRoles;

public interface IFgsUserRoleReadRepository
{
    Task<FgsUserRoleDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsUserRoleSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsUserRoleListFilters filters,
        CancellationToken cancellationToken = default);
}
