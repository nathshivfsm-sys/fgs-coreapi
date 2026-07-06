using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal HTTP client for tenant storage operations owned by FileService.
/// </summary>
public interface IFileTenantClient
{
    [Post("/api/v1/tenant/{tenantId}/bucket")]
    Task<Fgs.Contracts.Api.ApiResponse<ProvisionTenantBucketResponse>> ProvisionBucketAsync(
        long tenantId,
        [Body] ProvisionTenantBucketRequest request,
        CancellationToken cancellationToken = default);
}
