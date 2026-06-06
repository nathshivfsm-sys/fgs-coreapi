using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Requests;
using Fgs.Consumer.Application.Features.Notifications.Commands.ProcessCompanySignupInviteEmail;
using Fgs.Messaging.Consumer;
using Moq;

namespace Fgs.Consumer.Tests;

public sealed class ProcessCompanySignupInviteEmailCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsNotificationDispatchClient()
    {
        var client = new Mock<INotificationDispatchClient>();
        client.Setup(c => c.DispatchAsync(It.IsAny<DispatchNotificationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent));

        var handler = new ProcessCompanySignupInviteEmailCommandHandler(client.Object);
        var evt = new CompanySignupInviteEmailEvent(
            1, 2, Guid.NewGuid(), Guid.NewGuid(), "user@example.com",
            CommunicationTemplateCodes.CompanyAdminInvitation, "User", "FGS", "https://invite", "72", "support@fgs.example");

        await handler.Handle(
            new ProcessCompanySignupInviteEmailCommand(evt, CreateContext()),
            CancellationToken.None);

        client.Verify(
            c => c.DispatchAsync(
                It.Is<DispatchNotificationRequest>(r =>
                    r.Source == NotificationDispatchSource.IntegrationEvent
                    && r.RoutingKey == IntegrationEventRoutingKeys.CompanySignupInviteEmail
                    && r.MessageId == "message-1"
                    && r.CorrelationId == "correlation-1"
                    && !string.IsNullOrWhiteSpace(r.Payload)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static ConsumerMessageContext CreateContext() => new()
    {
        RoutingKey = IntegrationEventRoutingKeys.CompanySignupInviteEmail,
        MessageId = "message-1",
        CorrelationId = "correlation-1",
        RawBody = "{}"
    };
}
