using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.Requests;
using MediatR;

namespace Fgs.Consumer.Application.Features.TenantProvisioning.Commands.ProcessTenantProvisionRequested;

public sealed class ProcessTenantProvisionRequestedCommandHandler(ISetupProvisioningClient setupClient)
    : IRequestHandler<ProcessTenantProvisionRequestedCommand>
{
    public async Task Handle(
        ProcessTenantProvisionRequestedCommand request,
        CancellationToken cancellationToken)
    {
        var evt = request.Event;
        var provisionRequest = new ProvisionTenantRequest(
            evt.TenantId,
            evt.CompanyId,
            evt.TenantCode,
            evt.CorrelationId,
            evt.UserId);

        var response = await setupClient.ProvisionTenantAsync(provisionRequest, cancellationToken);
        if (!response.Success)
        {
            var message = response.Errors.Count > 0
                ? string.Join("; ", response.Errors)
                : "Tenant provisioning failed.";
            throw new InvalidOperationException(message);
        }
    }
}
