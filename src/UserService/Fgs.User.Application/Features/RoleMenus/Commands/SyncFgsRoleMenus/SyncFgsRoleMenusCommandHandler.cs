using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RoleMenus.Commands.SyncFgsRoleMenus;

public sealed class SyncFgsRoleMenusCommandHandler(
    IFgsRoleMenuWriteService writeService,
    ILogger<SyncFgsRoleMenusCommandHandler> logger)
    : IRequestHandler<SyncFgsRoleMenusCommand, ApiResponse<IReadOnlyList<FgsRoleMenuDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsRoleMenuDetailDto>>> Handle(
        SyncFgsRoleMenusCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.SyncAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Synced menus for role {RoleId}; assignment count {Count}",
            request.Dto.RoleId,
            result.Count);
        return ApiResponse<IReadOnlyList<FgsRoleMenuDetailDto>>.Ok(result);
    }
}
