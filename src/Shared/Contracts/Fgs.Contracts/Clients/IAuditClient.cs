using Fgs.Contracts.CredentialAudit;
using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Refit client for the Audit service credentialaudit endpoint.
/// Consumer-only: producer services must enqueue audit events via the outbox pattern.
/// </summary>
public interface IAuditClient
{
    [Post("/api/v1/credentialaudit")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> RecordCredentialAuditAsync(
        [Body] RecordCredentialAuditRequest request,
        CancellationToken cancellationToken = default);
}
