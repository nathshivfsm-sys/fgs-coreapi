using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal HTTP client for company operations owned by UserService.
/// </summary>
public interface IUserCompanyClient
{
    [Get("/api/v1/company")]
    Task<Fgs.Contracts.Api.ApiResponse<IReadOnlyList<TenantCompanyDto>>> GetCompaniesAsync(
        [Query] long tenantId,
        CancellationToken cancellationToken = default);
}
