using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Application.Features.ServiceSetups.Commands.UpdateFgsTenantServiceSetup;

public sealed class UpdateFgsTenantServiceSetupCommandHandler(
    IFgsTenantServiceSetupWriteService writeService,
    ILogger<UpdateFgsTenantServiceSetupCommandHandler> logger)
    : IRequestHandler<UpdateFgsTenantServiceSetupCommand, ApiResponse<FgsTenantServiceSetupDetailDto>>
{
    public async Task<ApiResponse<FgsTenantServiceSetupDetailDto>> Handle(
        UpdateFgsTenantServiceSetupCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Updated service setup for tenant {TenantId} company {CompanyId}",
            result.TenantId,
            result.CompanyId);
        return ApiResponse<FgsTenantServiceSetupDetailDto>.Ok(result);
    }
}
