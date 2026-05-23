namespace Fgs.User.Application.Abstractions.Provisioning;

public interface ITenantDataSeedingEngine
{
    Task SeedTenantDataAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken = default);
}
