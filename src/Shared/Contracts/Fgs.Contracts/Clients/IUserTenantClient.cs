using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal HTTP client for tenant lifecycle operations owned by UserService.
/// </summary>
public interface IUserTenantClient
{
    [Get("/api/v1/tenant/{tenantId}")]
    Task<Fgs.Contracts.Api.ApiResponse<TenantDto>> GetTenantAsync(long tenantId, CancellationToken cancellationToken = default);

    [Get("/api/v1/tenant/{tenantId}/companies")]
    Task<Fgs.Contracts.Api.ApiResponse<IReadOnlyList<TenantCompanyDto>>> GetCompaniesAsync(
        long tenantId,
        CancellationToken cancellationToken = default);

    [Patch("/api/v1/tenant/{tenantId}/status")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> UpdateStatusAsync(
        long tenantId,
        [Body] UpdateTenantStatusRequest request,
        CancellationToken cancellationToken = default);

    [Patch("/api/v1/tenant/{tenantId}/storage-bucket")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> UpdateStorageBucketAsync(
        long tenantId,
        [Body] UpdateTenantStorageBucketRequest request,
        CancellationToken cancellationToken = default);
}
