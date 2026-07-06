using Fgs.Contracts.Requests;
using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal HTTP client for SetupService operations consumed by other FGS services.
/// </summary>
public interface ISetupClient
{
    [Post("/api/v1/tenant/{tenantId}/companies/{companyId}/business-type")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> AddCompanyBusinessTypesAsync(
        long tenantId,
        long companyId,
        [Body] AddCompanyBusinessTypesRequest request,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/communication-template/active")]
    Task<Fgs.Contracts.Api.ApiResponse<CommunicationTemplateDto>> GetActiveTemplateAsync(
        [Query] long? tenantId,
        [Query] long? companyId,
        [Query] string templateType,
        [Query] string code,
        [Header(InternalServiceHeaders.ServiceKey)] string internalServiceKey,
        CancellationToken cancellationToken = default);

    [Post("/api/v1/tenant-provisioning")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> ProvisionTenantAsync(
        [Body] ProvisionTenantRequest request,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/credential/resolved")]
    Task<Fgs.Contracts.Api.ApiResponse<ResolvedCredentialConfigurationDto>> GetResolvedCredentialsAsync(
        [Header(InternalServiceHeaders.ServiceKey)] string internalServiceKey,
        [Header(InternalServiceHeaders.ServiceName)] string? serviceName = null,
        CancellationToken cancellationToken = default);
}

public sealed record AddCompanyBusinessTypesRequest(
    IReadOnlyList<int> BusinessTypeIds,
    Guid CompanyGuid,
    string Code,
    string Name,
    bool IsActive = true);

public sealed record ResolvedCredentialConfigurationDto(
    IReadOnlyDictionary<string, string> Values);
