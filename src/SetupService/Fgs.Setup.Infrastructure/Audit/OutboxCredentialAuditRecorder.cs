using Fgs.Contracts.CredentialAudit;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Outbox;
using Fgs.Setup.Application.Abstractions.Credentials;

namespace Fgs.Setup.Infrastructure.Audit;

public sealed class OutboxCredentialAuditRecorder(IOutboxWriter outboxWriter) : ICredentialAuditRecorder
{
    public Task RecordAsync(RecordCredentialAuditRequest request, CancellationToken cancellationToken = default) =>
        outboxWriter.EnqueueCredentialAuditAsync(request, Guid.NewGuid(), cancellationToken);
}
