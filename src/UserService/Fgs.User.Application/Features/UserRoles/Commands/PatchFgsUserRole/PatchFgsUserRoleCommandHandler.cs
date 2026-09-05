using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.UserRoles.Commands.PatchFgsUserRole;

public sealed class PatchFgsUserRoleCommandHandler(
    IFgsUserRoleWriteService writeService,
    ILogger<PatchFgsUserRoleCommandHandler> logger)
    : IRequestHandler<PatchFgsUserRoleCommand, ApiResponse<FgsUserRoleDetailDto>>
{
    public async Task<ApiResponse<FgsUserRoleDetailDto>> Handle(
        PatchFgsUserRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched user-role assignment {UserRoleId}", result.Id);
        return ApiResponse<FgsUserRoleDetailDto>.Ok(result);
    }
}
