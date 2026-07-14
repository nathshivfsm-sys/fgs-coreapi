using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Permissions;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Permissions.Commands.UpdateFgsPermission;

public sealed class UpdateFgsPermissionCommandHandler(
    IFgsPermissionWriteService writeService,
    ILogger<UpdateFgsPermissionCommandHandler> logger)
    : IRequestHandler<UpdateFgsPermissionCommand, ApiResponse<FgsPermissionDetailDto>>
{
    public async Task<ApiResponse<FgsPermissionDetailDto>> Handle(
        UpdateFgsPermissionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated permission {PermissionId}", result.Id);
        return ApiResponse<FgsPermissionDetailDto>.Ok(result);
    }
}
