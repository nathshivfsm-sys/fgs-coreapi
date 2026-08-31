using Fgs.User.Application.Features.TenantMenus.Dtos;

namespace Fgs.User.Application.Abstractions.TenantMenus;

public interface IFgsTenantMenuWriteService
{
    Task<IReadOnlyList<FgsTenantMenuDetailDto>> SyncAsync(
        FgsTenantMenuSyncDto dto,
        CancellationToken cancellationToken = default);
}
