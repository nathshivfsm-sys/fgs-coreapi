using Fgs.User.Application.Features.RoleMenus.Dtos;

namespace Fgs.User.Application.Abstractions.RoleMenus;

public interface IFgsRoleMenuWriteService
{
    Task<IReadOnlyList<FgsRoleMenuDetailDto>> SyncAsync(
        FgsRoleMenuSyncDto dto,
        CancellationToken cancellationToken = default);
}
