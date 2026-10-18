using Fgs.Contracts.IntegrationEvents;
using Fgs.Setup.Application.Features.TenantProvisioning;

namespace Fgs.Setup.Application.Abstractions.Provisioning;

public interface ITenantProvisioningOrchestrator
{
    Task ProvisionAsync(TenantProvisionRequestedEvent request, CancellationToken cancellationToken = default);

    Task<TenantDataSeedResult> SyncMissingCatalogAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken = default);
}
