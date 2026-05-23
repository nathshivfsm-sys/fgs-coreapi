using Fgs.User.Application.IntegrationEvents;

namespace Fgs.User.Application.Abstractions.Provisioning;

public interface ITenantProvisioningOrchestrator
{
    Task ProvisionAsync(TenantProvisionRequestedEvent request, CancellationToken cancellationToken = default);
}
