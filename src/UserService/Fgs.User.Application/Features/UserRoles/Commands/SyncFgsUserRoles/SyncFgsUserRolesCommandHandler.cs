using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.UserRoles.Commands.SyncFgsUserRoles;

public sealed class SyncFgsUserRolesCommandHandler(
    IFgsUserRoleWriteService writeService,
    ILogger<SyncFgsUserRolesCommandHandler> logger)
    : IRequestHandler<SyncFgsUserRolesCommand, ApiResponse<IReadOnlyList<FgsUserRoleDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUserRoleDetailDto>>> Handle(
        SyncFgsUserRolesCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.SyncAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Synced roles for user {UserId}; assignment count {Count}",
            request.Dto.UserId,
            result.Count);
        return ApiResponse<IReadOnlyList<FgsUserRoleDetailDto>>.Ok(result);
    }
}
