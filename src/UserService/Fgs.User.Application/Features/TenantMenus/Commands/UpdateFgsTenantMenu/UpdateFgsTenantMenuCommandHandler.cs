using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.TenantMenus.Commands.UpdateFgsTenantMenu;

public sealed class UpdateFgsTenantMenuCommandHandler(
    IFgsTenantMenuWriteService writeService,
    ILogger<UpdateFgsTenantMenuCommandHandler> logger)
    : IRequestHandler<UpdateFgsTenantMenuCommand, ApiResponse<FgsTenantMenuDetailDto>>
{
    public async Task<ApiResponse<FgsTenantMenuDetailDto>> Handle(
        UpdateFgsTenantMenuCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated tenant menu {TenantMenuId}", result.Id);
        return ApiResponse<FgsTenantMenuDetailDto>.Ok(result);
    }
}
