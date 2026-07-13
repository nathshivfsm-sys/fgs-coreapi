using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Roles.Commands.UpdateFgsRole;

public sealed class UpdateFgsRoleCommandHandler(
    IFgsRoleWriteService writeService,
    ILogger<UpdateFgsRoleCommandHandler> logger)
    : IRequestHandler<UpdateFgsRoleCommand, ApiResponse<FgsRoleDetailDto>>
{
    public async Task<ApiResponse<FgsRoleDetailDto>> Handle(
        UpdateFgsRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated role {RoleId}", result.Id);
        return ApiResponse<FgsRoleDetailDto>.Ok(result);
    }
}
