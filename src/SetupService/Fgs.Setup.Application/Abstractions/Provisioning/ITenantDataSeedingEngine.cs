using Fgs.Setup.Application.Features.TenantProvisioning;

namespace Fgs.Setup.Application.Abstractions.Provisioning;

public interface ITenantDataSeedingEngine
{
    Task<TenantDataSeedResult> SeedTenantDataAsync(
        long tenantId,
        long companyId,
        IReadOnlyList<int>? gloBusinessTypeIds = null,
        CancellationToken cancellationToken = default);
}
