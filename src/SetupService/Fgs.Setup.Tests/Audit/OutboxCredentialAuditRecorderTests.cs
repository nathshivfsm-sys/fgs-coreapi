using Fgs.Contracts.CredentialAudit;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Abstractions;
using Fgs.Setup.Infrastructure.Audit;
using Moq;

namespace Fgs.Setup.Tests.Audit;

public sealed class OutboxCredentialAuditRecorderTests
{
    [Fact]
    public async Task RecordAsync_EnqueuesCredentialAuditRequestedEvent()
    {
        var outboxWriter = new Mock<IOutboxWriter>();
        var credentialId = Guid.NewGuid();
        var request = new RecordCredentialAuditRequest(
            TenantId: 10,
            CompanyId: 1,
            CredentialId: credentialId,
            ActionType: CredentialAuditActions.Created,
            Remarks: "Credential created.",
            CreatedBy: "user-1");

        var recorder = new OutboxCredentialAuditRecorder(outboxWriter.Object);
        await recorder.RecordAsync(request, CancellationToken.None);

        outboxWriter.Verify(
            w => w.EnqueueAsync(
                IntegrationEventTypes.CredentialAuditRequested,
                It.Is<string>(payload =>
                    payload.Contains(CredentialAuditActions.Created)
                    && payload.Contains("Credential created.")
                    && payload.Contains(credentialId.ToString())),
                It.IsAny<Guid>(),
                10L,
                1L,
                IntegrationEventTypes.AggregateTypes.Credential,
                credentialId.ToString(),
                null,
                IntegrationEventExchanges.AuditEvents,
                IntegrationEventRoutingKeys.CredentialAuditRequested,
                null,
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
