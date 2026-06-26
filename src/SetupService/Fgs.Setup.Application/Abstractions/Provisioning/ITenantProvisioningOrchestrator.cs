using Fgs.Contracts.IntegrationEvents;

namespace Fgs.Setup.Application.Abstractions.Provisioning;

public interface ITenantProvisioningOrchestrator
{
    Task ProvisionAsync(TenantProvisionRequestedEvent request, CancellationToken cancellationToken = default);
}
