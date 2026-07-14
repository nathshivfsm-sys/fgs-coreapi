using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RolePermissions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RolePermissions.Commands.DeleteFgsRolePermission;

public sealed class DeleteFgsRolePermissionCommandHandler(
    IFgsRolePermissionWriteService writeService,
    ILogger<DeleteFgsRolePermissionCommandHandler> logger)
    : IRequestHandler<DeleteFgsRolePermissionCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        DeleteFgsRolePermissionCommand request,
        CancellationToken cancellationToken)
    {
        await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Removed role permission assignment {Id}", request.Id);
        return ApiResponse<object>.Ok(new object());
    }
}
