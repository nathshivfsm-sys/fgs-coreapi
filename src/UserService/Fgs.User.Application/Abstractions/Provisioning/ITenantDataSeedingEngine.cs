using Fgs.User.Application.TenantProvisioning;

namespace Fgs.User.Application.Abstractions.Provisioning;

public interface ITenantDataSeedingEngine
{
    Task<TenantDataSeedResult> SeedTenantDataAsync(
        long tenantId,
        long companyId,
        IReadOnlyList<int>? gloBusinessTypeIds = null,
        CancellationToken cancellationToken = default);
}
