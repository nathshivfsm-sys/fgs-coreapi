using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RoleMenus.Commands.UpdateFgsRoleMenu;

public sealed class UpdateFgsRoleMenuCommandHandler(
    IFgsRoleMenuWriteService writeService,
    ILogger<UpdateFgsRoleMenuCommandHandler> logger)
    : IRequestHandler<UpdateFgsRoleMenuCommand, ApiResponse<FgsRoleMenuDetailDto>>
{
    public async Task<ApiResponse<FgsRoleMenuDetailDto>> Handle(
        UpdateFgsRoleMenuCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated role menu {RoleMenuId}", result.Id);
        return ApiResponse<FgsRoleMenuDetailDto>.Ok(result);
    }
}
