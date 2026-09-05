using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RolePermissions.Commands.UpdateFgsRolePermission;

public sealed class UpdateFgsRolePermissionCommandHandler(
    IFgsRolePermissionWriteService writeService,
    ILogger<UpdateFgsRolePermissionCommandHandler> logger)
    : IRequestHandler<UpdateFgsRolePermissionCommand, ApiResponse<FgsRolePermissionDetailDto>>
{
    public async Task<ApiResponse<FgsRolePermissionDetailDto>> Handle(
        UpdateFgsRolePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated role-permission assignment {RolePermissionId}", result.Id);
        return ApiResponse<FgsRolePermissionDetailDto>.Ok(result);
    }
}
