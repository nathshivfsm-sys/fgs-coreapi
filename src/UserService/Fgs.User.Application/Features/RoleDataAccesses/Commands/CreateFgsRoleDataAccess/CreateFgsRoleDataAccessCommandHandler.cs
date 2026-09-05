using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.RoleDataAccesses.Commands.CreateFgsRoleDataAccess;

public sealed class CreateFgsRoleDataAccessCommandHandler(
    IFgsRoleDataAccessWriteService writeService,
    ILogger<CreateFgsRoleDataAccessCommandHandler> logger)
    : IRequestHandler<CreateFgsRoleDataAccessCommand, ApiResponse<FgsRoleDataAccessDetailDto>>
{
    public async Task<ApiResponse<FgsRoleDataAccessDetailDto>> Handle(
        CreateFgsRoleDataAccessCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created role-data-access assignment {RoleDataAccessId} for role {FgsRoleId} dataAccess {FgsDataAccessId}",
            result.Id,
            result.FgsRoleId,
            result.FgsDataAccessId);
        return ApiResponse<FgsRoleDataAccessDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
