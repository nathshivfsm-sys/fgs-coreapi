using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Commands.UpdateFgsTenantServiceAccountsSetup;

public sealed class UpdateFgsTenantServiceAccountsSetupCommandHandler(
    IFgsTenantServiceAccountsSetupWriteService writeService,
    ILogger<UpdateFgsTenantServiceAccountsSetupCommandHandler> logger)
    : IRequestHandler<UpdateFgsTenantServiceAccountsSetupCommand, ApiResponse<FgsTenantServiceAccountsSetupDetailDto>>
{
    public async Task<ApiResponse<FgsTenantServiceAccountsSetupDetailDto>> Handle(
        UpdateFgsTenantServiceAccountsSetupCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Updated service accounts setup for tenant {TenantId} company {CompanyId}",
            result.TenantId,
            result.CompanyId);
        return ApiResponse<FgsTenantServiceAccountsSetupDetailDto>.Ok(result);
    }
}
