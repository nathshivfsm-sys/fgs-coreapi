using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.CredentialAudit;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Consumer.Application.Features.Audit.Commands.ProcessCredentialAuditRequested;
using Fgs.Messaging.Consumer;
using Moq;

namespace Fgs.Consumer.Tests;

public sealed class ProcessCredentialAuditRequestedCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsAuditClient()
    {
        var client = new Mock<IAuditClient>();
        client.Setup(c => c.RecordCredentialAuditAsync(It.IsAny<RecordCredentialAuditRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object(), ApiStatusCodes.Created));

        var handler = new ProcessCredentialAuditRequestedCommandHandler(client.Object);
        var credentialId = Guid.NewGuid();
        var evt = new CredentialAuditRequestedEvent(
            10,
            1,
            credentialId,
            CredentialAuditActions.Created,
            "Credential created.",
            CreatedBy: "user-1");

        await handler.Handle(
            new ProcessCredentialAuditRequestedCommand(evt, CreateContext()),
            CancellationToken.None);

        client.Verify(
            c => c.RecordCredentialAuditAsync(
                It.Is<RecordCredentialAuditRequest>(r =>
                    r.TenantId == 10
                    && r.CompanyId == 1
                    && r.CredentialId == credentialId
                    && r.ActionType == CredentialAuditActions.Created
                    && r.Remarks == "Credential created."
                    && r.CreatedBy == "user-1"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenClientFails_Throws()
    {
        var client = new Mock<IAuditClient>();
        client.Setup(c => c.RecordCredentialAuditAsync(It.IsAny<RecordCredentialAuditRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Fail(["failed"], ApiStatusCodes.InternalServerError));

        var handler = new ProcessCredentialAuditRequestedCommandHandler(client.Object);

        var act = () => handler.Handle(
            new ProcessCredentialAuditRequestedCommand(
                new CredentialAuditRequestedEvent(10, 1, Guid.NewGuid(), CredentialAuditActions.Updated),
                CreateContext()),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private static ConsumerMessageContext CreateContext() => new()
    {
        RoutingKey = IntegrationEventRoutingKeys.CredentialAuditRequested,
        MessageId = Guid.NewGuid().ToString("N"),
        CorrelationId = Guid.NewGuid().ToString(),
        RawBody = "{}"
    };
}
