using Fgs.Contracts.CredentialAudit;
using Fgs.Setup.Application.Abstractions.Credentials;

namespace Fgs.Setup.Infrastructure.Audit;

public sealed class NoOpCredentialAuditRecorder : ICredentialAuditRecorder
{
    public Task RecordAsync(RecordCredentialAuditRequest request, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
