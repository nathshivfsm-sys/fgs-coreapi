using Fgs.Contracts.Audit;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Outbox;
using Fgs.Setup.Application.Abstractions.Employees;

namespace Fgs.Setup.Infrastructure.Audit;

public sealed class OutboxEmployeeAuditRecorder(IOutboxWriter outboxWriter) : IEmployeeAuditRecorder
{
    public Task RecordAsync(RecordAuditEventRequest request, CancellationToken cancellationToken = default) =>
        outboxWriter.EnqueueAuditEventAsync(request, Guid.NewGuid(), cancellationToken);
}
