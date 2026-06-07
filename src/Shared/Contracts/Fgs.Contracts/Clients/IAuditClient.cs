using Fgs.Contracts.CredentialAudit;
using Refit;

namespace Fgs.Contracts.Clients;

public interface IAuditClient
{
    [Post("/api/v1/credential-audits")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> RecordCredentialAuditAsync(
        [Body] RecordCredentialAuditRequest request,
        CancellationToken cancellationToken = default);
}
