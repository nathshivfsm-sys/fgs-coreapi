using Fgs.User.Application.Features.RoleDataAccesses.Dtos;

namespace Fgs.User.Application.Abstractions.RoleDataAccesses;

public interface IFgsRoleDataAccessWriteService
{
    Task<FgsRoleDataAccessDetailDto> CreateAsync(
        FgsRoleDataAccessCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsRoleDataAccessDetailDto> UpdateAsync(
        long id,
        FgsRoleDataAccessUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsRoleDataAccessDetailDto> PatchAsync(
        long id,
        FgsRoleDataAccessPatchDto dto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Syncs the role's data-access assignments to the given set (add missing, keep existing, remove extras).
    /// </summary>
    Task<IReadOnlyList<FgsRoleDataAccessDetailDto>> SyncAsync(
        FgsRoleDataAccessSyncDto dto,
        CancellationToken cancellationToken = default);
}
