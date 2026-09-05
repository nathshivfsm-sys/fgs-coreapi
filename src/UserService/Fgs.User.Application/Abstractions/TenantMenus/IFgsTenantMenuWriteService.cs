using Fgs.User.Application.Features.TenantMenus.Dtos;

namespace Fgs.User.Application.Abstractions.TenantMenus;

public interface IFgsTenantMenuWriteService
{
    Task<FgsTenantMenuDetailDto> CreateAsync(
        FgsTenantMenuCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsTenantMenuDetailDto> UpdateAsync(
        long id,
        FgsTenantMenuUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsTenantMenuDetailDto> PatchAsync(
        long id,
        FgsTenantMenuPatchDto dto,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsTenantMenuDetailDto>> SyncAsync(
        FgsTenantMenuSyncDto dto,
        CancellationToken cancellationToken = default);
}
