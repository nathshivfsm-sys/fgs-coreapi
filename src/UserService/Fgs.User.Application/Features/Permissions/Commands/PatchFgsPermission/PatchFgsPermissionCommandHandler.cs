using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Permissions;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Permissions.Commands.PatchFgsPermission;

public sealed class PatchFgsPermissionCommandHandler(
    IFgsPermissionWriteService writeService,
    ILogger<PatchFgsPermissionCommandHandler> logger)
    : IRequestHandler<PatchFgsPermissionCommand, ApiResponse<FgsPermissionDetailDto>>
{
    public async Task<ApiResponse<FgsPermissionDetailDto>> Handle(
        PatchFgsPermissionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched permission {PermissionId}", result.Id);
        return ApiResponse<FgsPermissionDetailDto>.Ok(result);
    }
}
