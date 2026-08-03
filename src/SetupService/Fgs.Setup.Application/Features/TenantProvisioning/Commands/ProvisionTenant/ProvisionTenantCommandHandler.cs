using Fgs.Contracts.Api;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.Setup.Application.Abstractions.Provisioning;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.Application.Features.TenantProvisioning.Commands.ProvisionTenant;

public sealed class ProvisionTenantCommandHandler(
    ITenantProvisioningOrchestrator orchestrator,
    IOptions<CredentialDistributionOptions> distributionOptions)
    : IRequestHandler<ProvisionTenantCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        ProvisionTenantCommand command,
        CancellationToken cancellationToken)
    {
        if (!InternalServiceAuthorization.IsAuthorized(
                command.InternalServiceKey,
                distributionOptions.Value))
        {
            return ApiResponse<object>.Fail(
                ["Internal service key is missing or invalid."],
                ApiStatusCodes.Unauthorized);
        }

        var request = command.Request;
        var provisionEvent = new TenantProvisionRequestedEvent(
            request.TenantId,
            request.CompanyId,
            request.TenantCode,
            request.CorrelationId,
            request.UserId);

        await orchestrator.ProvisionAsync(provisionEvent, cancellationToken);
        return ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent);
    }
}
