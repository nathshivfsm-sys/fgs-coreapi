using Fgs.User.Application.Features.UserRoles.Dtos;

namespace Fgs.User.Application.Abstractions.UserRoles;

public interface IFgsUserRoleWriteService
{
    /// <summary>
    /// Syncs the user's roles to the given set (add missing, keep existing, remove extras).
    /// </summary>
    Task<IReadOnlyList<FgsUserRoleDetailDto>> SyncAsync(
        FgsUserRoleSyncDto dto,
        CancellationToken cancellationToken = default);
}
