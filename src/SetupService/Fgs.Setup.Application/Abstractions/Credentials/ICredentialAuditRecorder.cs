using Fgs.Contracts.CredentialAudit;

namespace Fgs.Setup.Application.Abstractions.Credentials;

public interface ICredentialAuditRecorder
{
    Task RecordAsync(RecordCredentialAuditRequest request, CancellationToken cancellationToken = default);
}
