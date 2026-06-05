using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal HTTP client for SetupService operations consumed by other FGS services.
/// </summary>
public interface ISetupClient
{
    [Post("/api/v1/tenants/{tenantId}/companies/{companyId}/business-types")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> AddCompanyBusinessTypesAsync(
        long tenantId,
        long companyId,
        [Body] AddCompanyBusinessTypesRequest request,
        CancellationToken cancellationToken = default);
}

public sealed record AddCompanyBusinessTypesRequest(
    IReadOnlyList<int> BusinessTypeIds,
    Guid CompanyGuid,
    string Code,
    string Name,
    bool IsActive = true);
