using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Features.Roles.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Roles.Commands.CreateFgsRole;

public sealed class CreateFgsRoleCommandHandler(
    IFgsRoleWriteService writeService,
    ILogger<CreateFgsRoleCommandHandler> logger)
    : IRequestHandler<CreateFgsRoleCommand, ApiResponse<FgsRoleDetailDto>>
{
    public async Task<ApiResponse<FgsRoleDetailDto>> Handle(
        CreateFgsRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created role {RoleId} with code {RoleCode}", result.Id, result.RoleCode);
        return ApiResponse<FgsRoleDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
