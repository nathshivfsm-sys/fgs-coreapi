using Fgs.User.Application.Features.RolePermissions.Dtos;

namespace Fgs.User.Application.Abstractions.RolePermissions;

public interface IFgsRolePermissionWriteService
{
    /// <summary>
    /// Syncs the role's permissions to the given set (add missing, keep existing, remove extras).
    /// </summary>
    Task<IReadOnlyList<FgsRolePermissionDetailDto>> SyncAsync(
        FgsRolePermissionSyncDto dto,
        CancellationToken cancellationToken = default);
}
