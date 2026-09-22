using Fgs.Contracts.Audit;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Abstractions;
using Fgs.Setup.Infrastructure.Audit;
using Moq;

namespace Fgs.Setup.Tests.Audit;

public sealed class OutboxEmployeeAuditRecorderTests
{
    [Fact]
    public async Task RecordAsync_EnqueuesAuditEventRequestedEvent()
    {
        var outboxWriter = new Mock<IOutboxWriter>();
        var request = new RecordAuditEventRequest(
            TenantId: 10,
            CompanyId: 1,
            EventCode: "EMPLOYEE_UPDATED",
            EventSource: "API",
            RecordType: "SYSTEM",
            EntityId: 42,
            Summary: "Employee updated.",
            EntityNumber: "EMP-001",
            UserName: "Alex",
            Details:
            [
                new RecordAuditEventDetailRequest("FIELD_CHANGE", "DisplayName", "Old", "New")
            ]);

        var recorder = new OutboxEmployeeAuditRecorder(outboxWriter.Object);
        await recorder.RecordAsync(request, CancellationToken.None);

        outboxWriter.Verify(
            w => w.EnqueueAsync(
                IntegrationEventTypes.AuditEventRequested,
                It.Is<string>(payload =>
                    payload.Contains("EMPLOYEE_UPDATED")
                    && payload.Contains("Employee updated.")
                    && payload.Contains("DisplayName")),
                It.IsAny<Guid>(),
                10L,
                1L,
                "SYSTEM",
                "42",
                null,
                IntegrationEventExchanges.AuditEvents,
                IntegrationEventRoutingKeys.AuditEventRequested,
                null,
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
