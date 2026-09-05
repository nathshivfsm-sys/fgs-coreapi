using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.TenantMenus.Commands.PatchFgsTenantMenu;

public sealed class PatchFgsTenantMenuCommandHandler(
    IFgsTenantMenuWriteService writeService,
    ILogger<PatchFgsTenantMenuCommandHandler> logger)
    : IRequestHandler<PatchFgsTenantMenuCommand, ApiResponse<FgsTenantMenuDetailDto>>
{
    public async Task<ApiResponse<FgsTenantMenuDetailDto>> Handle(
        PatchFgsTenantMenuCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched tenant menu {TenantMenuId}", result.Id);
        return ApiResponse<FgsTenantMenuDetailDto>.Ok(result);
    }
}
