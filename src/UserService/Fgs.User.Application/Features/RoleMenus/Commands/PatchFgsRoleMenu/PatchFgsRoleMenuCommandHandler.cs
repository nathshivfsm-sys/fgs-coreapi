using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RoleMenus.Commands.PatchFgsRoleMenu;

public sealed class PatchFgsRoleMenuCommandHandler(
    IFgsRoleMenuWriteService writeService,
    ILogger<PatchFgsRoleMenuCommandHandler> logger)
    : IRequestHandler<PatchFgsRoleMenuCommand, ApiResponse<FgsRoleMenuDetailDto>>
{
    public async Task<ApiResponse<FgsRoleMenuDetailDto>> Handle(
        PatchFgsRoleMenuCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched role menu {RoleMenuId}", result.Id);
        return ApiResponse<FgsRoleMenuDetailDto>.Ok(result);
    }
}
