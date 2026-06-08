using Fgs.Contracts.CredentialAudit;

namespace Fgs.Audit.Application.Abstractions;

public interface ICredentialAuditWriter
{
    Task WriteAsync(RecordCredentialAuditRequest request, CancellationToken cancellationToken = default);
}
