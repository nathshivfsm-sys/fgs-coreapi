using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Requests;
using Fgs.Consumer.Application.Features.Notifications.Commands.ProcessUserInvited;
using Fgs.Messaging.Consumer;
using Moq;

namespace Fgs.Consumer.Tests;

public sealed class ProcessUserInvitedCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsNotificationDispatchClient()
    {
        var client = new Mock<INotificationDispatchClient>();
        client.Setup(c => c.DispatchAsync(It.IsAny<DispatchNotificationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent));

        var handler = new ProcessUserInvitedCommandHandler(client.Object);
        var evt = new UserInvitedEvent(
            1,
            2,
            Guid.Parse("452d5537-4314-4a45-a772-94678f74e245"),
            "fgs_user75@yopmail.com",
            "Pralhad G",
            "https://example.com/invite?token=abc",
            "Acme");

        await handler.Handle(
            new ProcessUserInvitedCommand(evt, CreateContext()),
            CancellationToken.None);

        client.Verify(
            c => c.DispatchAsync(
                It.Is<DispatchNotificationRequest>(r =>
                    r.RoutingKey == IntegrationEventRoutingKeys.UserInvited
                    && r.MessageId == "message-1"
                    && r.CorrelationId == "correlation-1"
                    && r.Payload.Contains("fgs_user75@yopmail.com", StringComparison.Ordinal)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDispatchFails_Throws()
    {
        var client = new Mock<INotificationDispatchClient>();
        client.Setup(c => c.DispatchAsync(It.IsAny<DispatchNotificationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Fail(["Template not found."], ApiStatusCodes.BadRequest));

        var handler = new ProcessUserInvitedCommandHandler(client.Object);
        var evt = new UserInvitedEvent(
            1, 2, Guid.NewGuid(), "user@example.com", "User", "https://invite", "Acme");

        var act = () => handler.Handle(
            new ProcessUserInvitedCommand(evt, CreateContext()),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Template not found.");
    }

    private static ConsumerMessageContext CreateContext() => new()
    {
        RoutingKey = IntegrationEventRoutingKeys.UserInvited,
        MessageId = "message-1",
        CorrelationId = "correlation-1",
        RawBody = "{}"
    };
}
