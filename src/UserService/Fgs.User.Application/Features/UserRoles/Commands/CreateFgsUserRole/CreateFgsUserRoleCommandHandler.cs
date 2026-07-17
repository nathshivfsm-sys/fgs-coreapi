using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.UserRoles.Commands.CreateFgsUserRole;

public sealed class CreateFgsUserRoleCommandHandler(
    IFgsUserRoleWriteService writeService,
    ILogger<CreateFgsUserRoleCommandHandler> logger)
    : IRequestHandler<CreateFgsUserRoleCommand, ApiResponse<FgsUserRoleDetailDto>>
{
    public async Task<ApiResponse<FgsUserRoleDetailDto>> Handle(
        CreateFgsUserRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Assigned role {RoleId} to user {UserId}", result.FgsRoleId, result.UserId);
        return ApiResponse<FgsUserRoleDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
