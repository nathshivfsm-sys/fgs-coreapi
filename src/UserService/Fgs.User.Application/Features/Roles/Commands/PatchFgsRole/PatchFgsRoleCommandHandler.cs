using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Roles.Commands.PatchFgsRole;

public sealed class PatchFgsRoleCommandHandler(
    IFgsRoleWriteService writeService,
    ILogger<PatchFgsRoleCommandHandler> logger)
    : IRequestHandler<PatchFgsRoleCommand, ApiResponse<FgsRoleDetailDto>>
{
    public async Task<ApiResponse<FgsRoleDetailDto>> Handle(
        PatchFgsRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched role {RoleId}", result.Id);
        return ApiResponse<FgsRoleDetailDto>.Ok(result);
    }
}
