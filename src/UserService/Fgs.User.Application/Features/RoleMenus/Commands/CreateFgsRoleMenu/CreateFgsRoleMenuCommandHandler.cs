using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RoleMenus.Commands.CreateFgsRoleMenu;

public sealed class CreateFgsRoleMenuCommandHandler(
    IFgsRoleMenuWriteService writeService,
    ILogger<CreateFgsRoleMenuCommandHandler> logger)
    : IRequestHandler<CreateFgsRoleMenuCommand, ApiResponse<FgsRoleMenuDetailDto>>
{
    public async Task<ApiResponse<FgsRoleMenuDetailDto>> Handle(
        CreateFgsRoleMenuCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created role menu {RoleMenuId} for role {RoleId} menu {MenuId}",
            result.Id,
            result.RoleId,
            result.MenuId);
        return ApiResponse<FgsRoleMenuDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
