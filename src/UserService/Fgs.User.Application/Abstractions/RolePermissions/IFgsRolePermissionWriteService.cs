using Fgs.User.Application.Features.RolePermissions.Dtos;

namespace Fgs.User.Application.Abstractions.RolePermissions;

public interface IFgsRolePermissionWriteService
{
    Task<FgsRolePermissionDetailDto> CreateAsync(
        FgsRolePermissionCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsRolePermissionDetailDto> UpdateAsync(
        long id,
        FgsRolePermissionUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsRolePermissionDetailDto> PatchAsync(
        long id,
        FgsRolePermissionPatchDto dto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Syncs the role's permissions to the given set (add missing, keep existing, remove extras).
    /// </summary>
    Task<IReadOnlyList<FgsRolePermissionDetailDto>> SyncAsync(
        FgsRolePermissionSyncDto dto,
        CancellationToken cancellationToken = default);
}
