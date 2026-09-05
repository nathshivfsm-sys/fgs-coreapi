using Fgs.User.Application.Features.UserRoles.Dtos;

namespace Fgs.User.Application.Abstractions.UserRoles;

public interface IFgsUserRoleWriteService
{
    Task<FgsUserRoleDetailDto> CreateAsync(
        FgsUserRoleCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsUserRoleDetailDto> UpdateAsync(
        long id,
        FgsUserRoleUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsUserRoleDetailDto> PatchAsync(
        long id,
        FgsUserRolePatchDto dto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Syncs the user's roles to the given set (add missing, keep existing, remove extras).
    /// </summary>
    Task<IReadOnlyList<FgsUserRoleDetailDto>> SyncAsync(
        FgsUserRoleSyncDto dto,
        CancellationToken cancellationToken = default);
}
