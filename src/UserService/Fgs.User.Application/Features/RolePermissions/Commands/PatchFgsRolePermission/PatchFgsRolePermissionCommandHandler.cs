using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RolePermissions.Commands.PatchFgsRolePermission;

public sealed class PatchFgsRolePermissionCommandHandler(
    IFgsRolePermissionWriteService writeService,
    ILogger<PatchFgsRolePermissionCommandHandler> logger)
    : IRequestHandler<PatchFgsRolePermissionCommand, ApiResponse<FgsRolePermissionDetailDto>>
{
    public async Task<ApiResponse<FgsRolePermissionDetailDto>> Handle(
        PatchFgsRolePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched role-permission assignment {RolePermissionId}", result.Id);
        return ApiResponse<FgsRolePermissionDetailDto>.Ok(result);
    }
}
