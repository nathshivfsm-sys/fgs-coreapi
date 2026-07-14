using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RolePermissions.Commands.CreateFgsRolePermission;

public sealed class CreateFgsRolePermissionCommandHandler(
    IFgsRolePermissionWriteService writeService,
    ILogger<CreateFgsRolePermissionCommandHandler> logger)
    : IRequestHandler<CreateFgsRolePermissionCommand, ApiResponse<FgsRolePermissionDetailDto>>
{
    public async Task<ApiResponse<FgsRolePermissionDetailDto>> Handle(
        CreateFgsRolePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created role permission assignment {Id}", result.Id);
        return ApiResponse<FgsRolePermissionDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
