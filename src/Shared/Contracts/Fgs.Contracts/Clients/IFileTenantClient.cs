using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal HTTP client for tenant storage operations owned by FileService.
/// </summary>
public interface IFileTenantClient
{
    [Post("/api/v1/tenants/{tenantId}/bucket")]
    Task<ProvisionTenantBucketResponse> ProvisionBucketAsync(
        long tenantId,
        [Body] ProvisionTenantBucketRequest request,
        CancellationToken cancellationToken = default);

    [Post("/api/v1/tenants/{tenantId}/folders")]
    Task InitializeFoldersAsync(
        long tenantId,
        [Body] InitializeTenantFoldersRequest request,
        CancellationToken cancellationToken = default);
}
