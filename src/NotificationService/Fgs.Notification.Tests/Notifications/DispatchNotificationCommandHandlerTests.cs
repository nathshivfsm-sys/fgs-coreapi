using Fgs.Contracts.Api;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Requests;
using Fgs.Notification.Application.Features.Notifications.Commands.DispatchNotification;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Application.Notifications.Dispatch;
using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Domain.Notifications;
using Moq;
using System.Text.Json;

namespace Fgs.Notification.Tests.Notifications;

public sealed class DispatchNotificationCommandHandlerTests
{
    private readonly Mock<IIntegrationEventMapper> _mapper = new();
    private readonly Mock<IIdempotencyStore> _idempotency = new();
    private readonly Mock<INotificationDispatcher> _dispatcher = new();

    [Fact]
    public async Task Handle_EventShape_DispatchesMappedNotification()
    {
        _mapper.Setup(m => m.CanMap(IntegrationEventRoutingKeys.CompanySignupInviteEmail)).Returns(true);
        _idempotency.Setup(i => i.TryMarkProcessedAsync("msg-1", IntegrationEventRoutingKeys.CompanySignupInviteEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var dispatchRequest = new NotificationDispatchRequest(
            1,
            2,
            NotificationChannel.Email,
            CommunicationTemplateCodes.CompanyAdminInvitation,
            "user@example.com",
            new Dictionary<string, string>(),
            "corr-1",
            "msg-1");

        _mapper.Setup(m => m.Map(
                IntegrationEventRoutingKeys.CompanySignupInviteEmail,
                It.IsAny<string>(),
                "corr-1",
                "msg-1"))
            .Returns(dispatchRequest);

        _dispatcher.Setup(d => d.DispatchAsync(dispatchRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NotificationDispatchResult(true, "provider-id", null));

        var handler = CreateHandler();
        var payload = JsonSerializer.Serialize(new CompanySignupInviteEmailEvent(
            1, 2, Guid.NewGuid(), Guid.NewGuid(), "user@example.com",
            CommunicationTemplateCodes.CompanyAdminInvitation, "User", "FGS", "https://invite", "72", "support@fgs.example"));

        var response = await handler.Handle(
            new DispatchNotificationCommand(new DispatchNotificationRequest
            {
                RoutingKey = IntegrationEventRoutingKeys.CompanySignupInviteEmail,
                Payload = payload,
                CorrelationId = "corr-1",
                MessageId = "msg-1"
            }),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        _dispatcher.Verify(d => d.DispatchAsync(dispatchRequest, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ExplicitShape_DispatchesNotification()
    {
        _dispatcher.Setup(d => d.DispatchAsync(It.IsAny<NotificationDispatchRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NotificationDispatchResult(true, "provider-id", null));

        var handler = CreateHandler();
        var response = await handler.Handle(
            new DispatchNotificationCommand(new DispatchNotificationRequest
            {
                TenantId = 1,
                CompanyId = 2,
                Channel = "Email",
                TemplateCode = CommunicationTemplateCodes.CompanyAdminInvitation,
                Recipient = "user@example.com",
                Tokens = new Dictionary<string, string> { ["Name"] = "User" }
            }),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        _dispatcher.Verify(
            d => d.DispatchAsync(
                It.Is<NotificationDispatchRequest>(r =>
                    r.TenantId == 1
                    && r.CompanyId == 2
                    && r.Channel == NotificationChannel.Email
                    && r.TemplateCode == CommunicationTemplateCodes.CompanyAdminInvitation
                    && r.Recipient == "user@example.com"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidShape_ReturnsBadRequest()
    {
        var handler = CreateHandler();

        var response = await handler.Handle(
            new DispatchNotificationCommand(new DispatchNotificationRequest
            {
                RoutingKey = IntegrationEventRoutingKeys.CompanySignupInviteEmail
            }),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        _dispatcher.Verify(d => d.DispatchAsync(It.IsAny<NotificationDispatchRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private DispatchNotificationCommandHandler CreateHandler() =>
        new(
            new NotificationDispatchRequestResolver(_mapper.Object),
            _idempotency.Object,
            _dispatcher.Object);
}
