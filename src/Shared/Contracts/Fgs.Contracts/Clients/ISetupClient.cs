using Fgs.Contracts.Requests;
using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal HTTP client for SetupService operations consumed by other FGS services.
/// </summary>
public interface ISetupClient
{
    [Post("/api/v1/tenant/{tenantId}/companies/{companyId}/businesstype")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> AddCompanyBusinessTypesAsync(
        long tenantId,
        long companyId,
        [Body] AddCompanyBusinessTypesRequest request,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/communicationtemplate/active")]
    Task<Fgs.Contracts.Api.ApiResponse<CommunicationTemplateDto>> GetActiveTemplateAsync(
        [Query] long? tenantId,
        [Query] long? companyId,
        [Query] string templateType,
        [Query] string code,
        [Header(InternalServiceHeaders.ServiceKey)] string internalServiceKey,
        CancellationToken cancellationToken = default);

    [Post("/api/v1/tenantprovisioning")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> ProvisionTenantAsync(
        [Body] ProvisionTenantRequest request,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/credential/resolved")]
    Task<Fgs.Contracts.Api.ApiResponse<ResolvedCredentialConfigurationDto>> GetResolvedCredentialsAsync(
        [Header(InternalServiceHeaders.ServiceKey)] string internalServiceKey,
        [Header(InternalServiceHeaders.ServiceName)] string? serviceName = null,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/glbreak/lookup")]
    Task<Fgs.Contracts.Api.ApiResponse<IReadOnlyList<GlBreakLookupItemDto>>> LookupGlBreaksAsync(
        [Query] bool activeOnly,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/credential/company-provider")]
    Task<Fgs.Contracts.Api.ApiResponse<CompanyProviderCredentialDto>> GetCompanyProviderCredentialAsync(
        [Query] string providerCode,
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

/// <summary>
/// Id and name from Setup GL break lookup. Code and break level are ignored.
/// </summary>
public sealed record GlBreakLookupItemDto(
    long Id,
    string Name);

/// <summary>
/// Decrypted company credential fields for Authorize.net. Callers must not log these values.
/// </summary>
public sealed record CompanyProviderCredentialDto(
    string ApiLoginId,
    string PublicClientKey,
    string TransactionKey,
    bool UseSandbox);
