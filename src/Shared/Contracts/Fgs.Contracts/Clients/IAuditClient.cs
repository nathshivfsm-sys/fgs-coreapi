using Fgs.Contracts.Audit;
using Fgs.Contracts.CredentialAudit;
using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Refit client for Audit service endpoints.
/// Consumer-only: producer services must enqueue audit events via the outbox pattern.
/// </summary>
public interface IAuditClient
{
    [Post("/api/v1/credentialaudit")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> RecordCredentialAuditAsync(
        [Body] RecordCredentialAuditRequest request,
        CancellationToken cancellationToken = default);

    [Post("/api/v1/event")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> RecordAuditEventAsync(
        [Body] RecordAuditEventRequest request,
        CancellationToken cancellationToken = default);
}
