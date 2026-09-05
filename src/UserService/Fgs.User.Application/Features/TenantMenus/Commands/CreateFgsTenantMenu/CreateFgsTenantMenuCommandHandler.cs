using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.TenantMenus.Commands.CreateFgsTenantMenu;

public sealed class CreateFgsTenantMenuCommandHandler(
    IFgsTenantMenuWriteService writeService,
    ILogger<CreateFgsTenantMenuCommandHandler> logger)
    : IRequestHandler<CreateFgsTenantMenuCommand, ApiResponse<FgsTenantMenuDetailDto>>
{
    public async Task<ApiResponse<FgsTenantMenuDetailDto>> Handle(
        CreateFgsTenantMenuCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created tenant menu {TenantMenuId} for MenuId {MenuId}",
            result.Id,
            result.MenuId);
        return ApiResponse<FgsTenantMenuDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
