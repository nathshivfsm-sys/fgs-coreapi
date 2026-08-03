using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Commands.PatchFgsTenantServiceAccountsSetup;

public sealed class PatchFgsTenantServiceAccountsSetupCommandHandler(
    IFgsTenantServiceAccountsSetupWriteService writeService,
    ILogger<PatchFgsTenantServiceAccountsSetupCommandHandler> logger)
    : IRequestHandler<PatchFgsTenantServiceAccountsSetupCommand, ApiResponse<FgsTenantServiceAccountsSetupDetailDto>>
{
    public async Task<ApiResponse<FgsTenantServiceAccountsSetupDetailDto>> Handle(
        PatchFgsTenantServiceAccountsSetupCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Patched service accounts setup for tenant {TenantId} company {CompanyId}",
            result.TenantId,
            result.CompanyId);
        return ApiResponse<FgsTenantServiceAccountsSetupDetailDto>.Ok(result);
    }
}
