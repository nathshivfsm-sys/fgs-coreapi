using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RoleDataAccesses.Commands.UpdateFgsRoleDataAccess;

public sealed class UpdateFgsRoleDataAccessCommandHandler(
    IFgsRoleDataAccessWriteService writeService,
    ILogger<UpdateFgsRoleDataAccessCommandHandler> logger)
    : IRequestHandler<UpdateFgsRoleDataAccessCommand, ApiResponse<FgsRoleDataAccessDetailDto>>
{
    public async Task<ApiResponse<FgsRoleDataAccessDetailDto>> Handle(
        UpdateFgsRoleDataAccessCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated role-data-access assignment {RoleDataAccessId}", result.Id);
        return ApiResponse<FgsRoleDataAccessDetailDto>.Ok(result);
    }
}
