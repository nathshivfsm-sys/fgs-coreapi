using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Roles.Commands.CloneFgsRole;

public sealed class CloneFgsRoleCommandHandler(
    IFgsRoleWriteService writeService,
    ILogger<CloneFgsRoleCommandHandler> logger)
    : IRequestHandler<CloneFgsRoleCommand, ApiResponse<FgsRoleDetailDto>>
{
    public async Task<ApiResponse<FgsRoleDetailDto>> Handle(
        CloneFgsRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CloneAsync(request.SourceRoleId, request.Dto, cancellationToken);
        logger.LogInformation(
            "Cloned role {SourceRoleId} to role {RoleId} with code {RoleCode}",
            request.SourceRoleId,
            result.Id,
            result.RoleCode);
        return ApiResponse<FgsRoleDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
