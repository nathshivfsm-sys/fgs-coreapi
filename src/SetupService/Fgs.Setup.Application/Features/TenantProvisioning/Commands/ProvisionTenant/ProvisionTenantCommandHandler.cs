using Fgs.Contracts.Api;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Setup.Application.Abstractions.Provisioning;
using MediatR;

namespace Fgs.Setup.Application.Features.TenantProvisioning.Commands.ProvisionTenant;

public sealed class ProvisionTenantCommandHandler(ITenantProvisioningOrchestrator orchestrator)
    : IRequestHandler<ProvisionTenantCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        ProvisionTenantCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;
        var provisionEvent = new TenantProvisionRequestedEvent(
            request.TenantId,
            request.CompanyId,
            request.TenantCode,
            request.CorrelationId,
            request.UserId);

        try
        {
            await orchestrator.ProvisionAsync(provisionEvent, cancellationToken);
            return ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>.Fail(
                [ex.Message],
                ApiStatusCodes.InternalServerError);
        }
    }
}
