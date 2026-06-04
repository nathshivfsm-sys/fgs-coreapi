using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal HTTP client for communication template reads owned by SetupService.
/// </summary>
public interface ISetupTemplateClient
{
    [Get("/api/v1/communication-templates/active")]
    Task<CommunicationTemplateDto?> GetActiveTemplateAsync(
        [Query] long? tenantId,
        [Query] long? companyId,
        [Query] string templateType,
        [Query] string code,
        [Header(InternalServiceHeaders.ServiceKey)] string internalServiceKey,
        CancellationToken cancellationToken = default);
}
