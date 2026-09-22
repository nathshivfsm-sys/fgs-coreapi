using Fgs.Contracts.Api;
using Fgs.Contracts.Audit;
using Fgs.Contracts.Clients;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Consumer.Application.Features.Audit.Commands.ProcessAuditEventRequested;
using Fgs.Messaging.Consumer;
using Moq;

namespace Fgs.Consumer.Tests;

public sealed class ProcessAuditEventRequestedCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsAuditClient()
    {
        var client = new Mock<IAuditClient>();
        client.Setup(c => c.RecordAuditEventAsync(It.IsAny<RecordAuditEventRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object(), ApiStatusCodes.Created));

        var handler = new ProcessAuditEventRequestedCommandHandler(client.Object);
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

        await handler.Handle(
            new ProcessAuditEventRequestedCommand(new AuditEventRequestedEvent(request), CreateContext()),
            CancellationToken.None);

        client.Verify(
            c => c.RecordAuditEventAsync(
                It.Is<RecordAuditEventRequest>(r =>
                    r.TenantId == 10
                    && r.CompanyId == 1
                    && r.EventCode == "EMPLOYEE_UPDATED"
                    && r.RecordType == "SYSTEM"
                    && r.EntityId == 42
                    && r.Summary == "Employee updated."),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenClientFails_Throws()
    {
        var client = new Mock<IAuditClient>();
        client.Setup(c => c.RecordAuditEventAsync(It.IsAny<RecordAuditEventRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Fail(["failed"], ApiStatusCodes.InternalServerError));

        var handler = new ProcessAuditEventRequestedCommandHandler(client.Object);

        var act = () => handler.Handle(
            new ProcessAuditEventRequestedCommand(
                new AuditEventRequestedEvent(
                    new RecordAuditEventRequest(
                        10,
                        1,
                        "EMPLOYEE_UPDATED",
                        "API",
                        "SYSTEM",
                        1,
                        "Employee updated.")),
                CreateContext()),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private static ConsumerMessageContext CreateContext() => new()
    {
        RoutingKey = IntegrationEventRoutingKeys.AuditEventRequested,
        MessageId = Guid.NewGuid().ToString("N"),
        CorrelationId = Guid.NewGuid().ToString(),
        RawBody = "{}"
    };
}
