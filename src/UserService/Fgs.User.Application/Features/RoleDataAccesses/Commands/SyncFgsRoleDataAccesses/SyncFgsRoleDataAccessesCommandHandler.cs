using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RoleDataAccesses.Commands.SyncFgsRoleDataAccesses;

public sealed class SyncFgsRoleDataAccessesCommandHandler(
    IFgsRoleDataAccessWriteService writeService,
    ILogger<SyncFgsRoleDataAccessesCommandHandler> logger)
    : IRequestHandler<SyncFgsRoleDataAccessesCommand, ApiResponse<IReadOnlyList<FgsRoleDataAccessDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsRoleDataAccessDetailDto>>> Handle(
        SyncFgsRoleDataAccessesCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.SyncAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Synced data accesses for role {RoleId}; assignment count {Count}",
            request.Dto.FgsRoleId,
            result.Count);
        return ApiResponse<IReadOnlyList<FgsRoleDataAccessDetailDto>>.Ok(result);
    }
}
