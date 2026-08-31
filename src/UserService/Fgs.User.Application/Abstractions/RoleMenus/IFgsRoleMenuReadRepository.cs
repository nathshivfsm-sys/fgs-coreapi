using Fgs.User.Application.Features.RoleMenus.Dtos;

namespace Fgs.User.Application.Abstractions.RoleMenus;

public interface IFgsRoleMenuReadRepository
{
    Task<IReadOnlyList<FgsRoleMenuDetailDto>> ListByRoleIdAsync(
        long roleId,
        CancellationToken cancellationToken = default);
}
