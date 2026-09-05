using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.UserRoles.Commands.UpdateFgsUserRole;

public sealed class UpdateFgsUserRoleCommandHandler(
    IFgsUserRoleWriteService writeService,
    ILogger<UpdateFgsUserRoleCommandHandler> logger)
    : IRequestHandler<UpdateFgsUserRoleCommand, ApiResponse<FgsUserRoleDetailDto>>
{
    public async Task<ApiResponse<FgsUserRoleDetailDto>> Handle(
        UpdateFgsUserRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated user-role assignment {UserRoleId}", result.Id);
        return ApiResponse<FgsUserRoleDetailDto>.Ok(result);
    }
}
