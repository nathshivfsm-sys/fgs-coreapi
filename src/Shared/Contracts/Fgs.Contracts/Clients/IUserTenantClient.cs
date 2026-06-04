using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal HTTP client for tenant lifecycle operations owned by UserService.
/// </summary>
public interface IUserTenantClient
{
    [Get("/api/v1/tenants/{tenantId}")]
    Task<TenantDto> GetTenantAsync(long tenantId, CancellationToken cancellationToken = default);

    [Get("/api/v1/tenants/{tenantId}/companies")]
    Task<IReadOnlyList<TenantCompanyDto>> GetCompaniesAsync(
        long tenantId,
        CancellationToken cancellationToken = default);

    [Patch("/api/v1/tenants/{tenantId}/status")]
    Task UpdateStatusAsync(
        long tenantId,
        [Body] UpdateTenantStatusRequest request,
        CancellationToken cancellationToken = default);

    [Patch("/api/v1/tenants/{tenantId}/storage-bucket")]
    Task UpdateStorageBucketAsync(
        long tenantId,
        [Body] UpdateTenantStorageBucketRequest request,
        CancellationToken cancellationToken = default);
}
