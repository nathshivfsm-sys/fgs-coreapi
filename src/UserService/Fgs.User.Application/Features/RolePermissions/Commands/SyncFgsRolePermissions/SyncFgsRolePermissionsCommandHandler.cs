using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RolePermissions.Commands.SyncFgsRolePermissions;

public sealed class SyncFgsRolePermissionsCommandHandler(
    IFgsRolePermissionWriteService writeService,
    ILogger<SyncFgsRolePermissionsCommandHandler> logger)
    : IRequestHandler<SyncFgsRolePermissionsCommand, ApiResponse<IReadOnlyList<FgsRolePermissionDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsRolePermissionDetailDto>>> Handle(
        SyncFgsRolePermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.SyncAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Synced permissions for role {RoleId}; assignment count {Count}",
            request.Dto.FgsRoleId,
            result.Count);
        return ApiResponse<IReadOnlyList<FgsRolePermissionDetailDto>>.Ok(result);
    }
}
