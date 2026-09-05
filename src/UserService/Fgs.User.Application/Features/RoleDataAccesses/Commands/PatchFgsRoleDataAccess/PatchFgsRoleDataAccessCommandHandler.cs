using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RoleDataAccesses.Commands.PatchFgsRoleDataAccess;

public sealed class PatchFgsRoleDataAccessCommandHandler(
    IFgsRoleDataAccessWriteService writeService,
    ILogger<PatchFgsRoleDataAccessCommandHandler> logger)
    : IRequestHandler<PatchFgsRoleDataAccessCommand, ApiResponse<FgsRoleDataAccessDetailDto>>
{
    public async Task<ApiResponse<FgsRoleDataAccessDetailDto>> Handle(
        PatchFgsRoleDataAccessCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched role-data-access assignment {RoleDataAccessId}", result.Id);
        return ApiResponse<FgsRoleDataAccessDetailDto>.Ok(result);
    }
}
