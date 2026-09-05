using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.TenantMenus.Commands.SyncFgsTenantMenus;

public sealed class SyncFgsTenantMenusCommandHandler(
    IFgsTenantMenuWriteService writeService,
    ILogger<SyncFgsTenantMenusCommandHandler> logger)
    : IRequestHandler<SyncFgsTenantMenusCommand, ApiResponse<IReadOnlyList<FgsTenantMenuDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsTenantMenuDetailDto>>> Handle(
        SyncFgsTenantMenusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.SyncAsync(request.Dto, cancellationToken);
        logger.LogInformation("Synced tenant menus; assignment count {Count}", result.Count);
        return ApiResponse<IReadOnlyList<FgsTenantMenuDetailDto>>.Ok(result);
    }
}
