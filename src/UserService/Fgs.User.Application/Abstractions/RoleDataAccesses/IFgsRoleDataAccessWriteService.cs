using Fgs.User.Application.Features.RoleDataAccesses.Dtos;

namespace Fgs.User.Application.Abstractions.RoleDataAccesses;

public interface IFgsRoleDataAccessWriteService
{
    /// <summary>
    /// Syncs the role's data-access assignments to the given set (add missing, keep existing, remove extras).
    /// </summary>
    Task<IReadOnlyList<FgsRoleDataAccessDetailDto>> SyncAsync(
        FgsRoleDataAccessSyncDto dto,
        CancellationToken cancellationToken = default);
}
