using Fgs.User.Application.Features.RoleMenus.Dtos;

namespace Fgs.User.Application.Abstractions.RoleMenus;

public interface IFgsRoleMenuWriteService
{
    Task<FgsRoleMenuDetailDto> CreateAsync(
        FgsRoleMenuCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsRoleMenuDetailDto> UpdateAsync(
        long id,
        FgsRoleMenuUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsRoleMenuDetailDto> PatchAsync(
        long id,
        FgsRoleMenuPatchDto dto,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsRoleMenuDetailDto>> SyncAsync(
        FgsRoleMenuSyncDto dto,
        CancellationToken cancellationToken = default);
}
