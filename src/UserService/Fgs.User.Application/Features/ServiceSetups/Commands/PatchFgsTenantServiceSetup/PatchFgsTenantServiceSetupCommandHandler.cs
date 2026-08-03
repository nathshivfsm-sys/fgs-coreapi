using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ServiceSetups.Commands.PatchFgsTenantServiceSetup;

public sealed class PatchFgsTenantServiceSetupCommandHandler(
    IFgsTenantServiceSetupWriteService writeService,
    ILogger<PatchFgsTenantServiceSetupCommandHandler> logger)
    : IRequestHandler<PatchFgsTenantServiceSetupCommand, ApiResponse<FgsTenantServiceSetupDetailDto>>
{
    public async Task<ApiResponse<FgsTenantServiceSetupDetailDto>> Handle(
        PatchFgsTenantServiceSetupCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Patched service setup for tenant {TenantId} company {CompanyId}",
            result.TenantId,
            result.CompanyId);
        return ApiResponse<FgsTenantServiceSetupDetailDto>.Ok(result);
    }
}
