using Fgs.User.Application.Features.RolePermissions.Dtos;

namespace Fgs.User.Application.Abstractions.RolePermissions;

public interface IFgsRolePermissionReadRepository
{
    Task<IReadOnlyList<FgsRolePermissionDetailDto>> ListByRoleIdAsync(
        long fgsRoleId,
        CancellationToken cancellationToken = default);
}
