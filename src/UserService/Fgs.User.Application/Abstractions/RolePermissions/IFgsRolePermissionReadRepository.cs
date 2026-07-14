using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.RolePermissions.Dtos;

namespace Fgs.User.Application.Abstractions.RolePermissions;

public interface IFgsRolePermissionReadRepository
{
    Task<FgsRolePermissionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<FgsRolePermissionSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsRolePermissionListFilters filters,
        CancellationToken cancellationToken = default);
}
