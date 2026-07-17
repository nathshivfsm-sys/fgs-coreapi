using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Permissions;
using Fgs.User.Application.Features.Permissions.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.Permissions.Commands.CreateFgsPermission;

public sealed class CreateFgsPermissionCommandHandler(
    IFgsPermissionWriteService writeService,
    ILogger<CreateFgsPermissionCommandHandler> logger)
    : IRequestHandler<CreateFgsPermissionCommand, ApiResponse<FgsPermissionDetailDto>>
{
    public async Task<ApiResponse<FgsPermissionDetailDto>> Handle(
        CreateFgsPermissionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created permission {PermissionId} with code {PermissionCode}",
            result.Id,
            result.PermissionCode);
        return ApiResponse<FgsPermissionDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
