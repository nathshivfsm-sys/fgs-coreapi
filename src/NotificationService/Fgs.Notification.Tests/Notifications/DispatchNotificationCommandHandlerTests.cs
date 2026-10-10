using Fgs.Contracts.Api;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Requests;
using Fgs.Notification.Application.Features.Notifications.Commands.DispatchNotification;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Application.Notifications.Dispatch;
using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Domain.Notifications;
using Fgs.Notification.Infrastructure.Notifications.Queues;
using Fgs.Notification.Infrastructure.Options;
using Microsoft.Extensions.Options;
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
        var dispatchRequest = CreateDispatchRequest();
        SetupMappedSignup(dispatchRequest);
        var calls = new List<string>();
        _idempotency.Setup(i => i.TryMarkProcessedAsync("msg-1", IntegrationEventRoutingKeys.CompanySignupInviteEmail, It.IsAny<CancellationToken>()))
            .Callback(() => calls.Add("claim"))
            .ReturnsAsync(true);
        _dispatcher.Setup(d => d.DispatchAsync(dispatchRequest, It.IsAny<CancellationToken>()))
            .Callback(() => calls.Add("dispatch"))
            .ReturnsAsync(new NotificationDispatchResult(true, "provider-id", null));

        var response = await CreateHandler().Handle(CreateSignupCommand(), CancellationToken.None);

        response.Success.Should().BeTrue();
        _dispatcher.Verify(d => d.DispatchAsync(dispatchRequest, It.IsAny<CancellationToken>()), Times.Once);
        _idempotency.Verify(
            i => i.TryMarkProcessedAsync("msg-1", IntegrationEventRoutingKeys.CompanySignupInviteEmail, It.IsAny<CancellationToken>()),
            Times.Once);
        _idempotency.Verify(i => i.ReleaseAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        calls.Should().Equal("claim", "dispatch");
    }

    [Fact]
    public async Task Handle_WhenClaimIsLost_DoesNotDispatch()
    {
        SetupMappedSignup(CreateDispatchRequest());
        _idempotency.Setup(i => i.TryMarkProcessedAsync("msg-1", IntegrationEventRoutingKeys.CompanySignupInviteEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var response = await CreateHandler().Handle(CreateSignupCommand(), CancellationToken.None);

        response.Success.Should().BeTrue();
        _dispatcher.Verify(d => d.DispatchAsync(It.IsAny<NotificationDispatchRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        _idempotency.Verify(i => i.ReleaseAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDispatchFails_ReleasesClaim()
    {
        var dispatchRequest = CreateDispatchRequest();
        SetupMappedSignup(dispatchRequest);
        _idempotency.Setup(i => i.TryMarkProcessedAsync("msg-1", IntegrationEventRoutingKeys.CompanySignupInviteEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _dispatcher.Setup(d => d.DispatchAsync(dispatchRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NotificationDispatchResult(false, null, "Provider unavailable."));

        var response = await CreateHandler().Handle(CreateSignupCommand(), CancellationToken.None);

        response.Success.Should().BeFalse();
        _idempotency.Verify(
            i => i.TryMarkProcessedAsync("msg-1", IntegrationEventRoutingKeys.CompanySignupInviteEmail, It.IsAny<CancellationToken>()),
            Times.Once);
        _idempotency.Verify(i => i.ReleaseAsync("msg-1", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTemplateNotFound_ReturnsNotFoundAndReleasesClaim()
    {
        var dispatchRequest = CreateDispatchRequest();
        SetupMappedSignup(dispatchRequest);
        _idempotency.Setup(i => i.TryMarkProcessedAsync("msg-1", IntegrationEventRoutingKeys.CompanySignupInviteEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _dispatcher.Setup(d => d.DispatchAsync(dispatchRequest, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CommunicationTemplateNotFoundException(
                1,
                2,
                CommunicationTemplateCodes.CompanyAdminInvitation,
                NotificationChannel.Email));

        var response = await CreateHandler().Handle(CreateSignupCommand(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
        _idempotency.Verify(i => i.ReleaseAsync("msg-1", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTemplateRenderingFails_ReturnsBadRequestWithMissingTokensAndReleasesClaim()
    {
        var dispatchRequest = CreateDispatchRequest();
        SetupMappedSignup(dispatchRequest);
        _idempotency.Setup(i => i.TryMarkProcessedAsync("msg-1", IntegrationEventRoutingKeys.CompanySignupInviteEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _dispatcher.Setup(d => d.DispatchAsync(dispatchRequest, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TemplateRenderingException("Template is missing required token value(s): InviteLink.")
            {
                MissingTokens = ["InviteLink"]
            });

        var response = await CreateHandler().Handle(CreateSignupCommand(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        response.Errors.Should().Contain("InviteLink");
        _idempotency.Verify(i => i.ReleaseAsync("msg-1", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NullPayload_ReturnsBadRequestWithoutDispatch()
    {
        var handler = new DispatchNotificationCommandHandler(
            new NotificationDispatchRequestResolver(new IntegrationEventMapper(Options.Create(new NotificationOptions()))),
            _idempotency.Object,
            _dispatcher.Object);

        var response = await handler.Handle(
            new DispatchNotificationCommand(new DispatchNotificationRequest
            {
                RoutingKey = IntegrationEventRoutingKeys.PasswordReset,
                Payload = "null",
                MessageId = "msg-null"
            }),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        _dispatcher.Verify(
            d => d.DispatchAsync(It.IsAny<NotificationDispatchRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ExplicitShape_DispatchesNotification()
    {
        _dispatcher.Setup(d => d.DispatchAsync(It.IsAny<NotificationDispatchRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NotificationDispatchResult(true, "provider-id", null));

        var response = await CreateHandler().Handle(
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
        _idempotency.Verify(
            i => i.TryMarkProcessedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_InvalidShape_ReturnsBadRequest()
    {
        var response = await CreateHandler().Handle(
            new DispatchNotificationCommand(new DispatchNotificationRequest
            {
                RoutingKey = IntegrationEventRoutingKeys.CompanySignupInviteEmail
            }),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        _dispatcher.Verify(d => d.DispatchAsync(It.IsAny<NotificationDispatchRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private void SetupMappedSignup(NotificationDispatchRequest dispatchRequest)
    {
        _mapper.Setup(m => m.CanMap(IntegrationEventRoutingKeys.CompanySignupInviteEmail)).Returns(true);
        _mapper.Setup(m => m.Map(
                IntegrationEventRoutingKeys.CompanySignupInviteEmail,
                It.IsAny<string>(),
                "corr-1",
                "msg-1"))
            .Returns(dispatchRequest);
    }

    private static NotificationDispatchRequest CreateDispatchRequest() =>
        new(
            1,
            2,
            NotificationChannel.Email,
            CommunicationTemplateCodes.CompanyAdminInvitation,
            "user@example.com",
            new Dictionary<string, string>(),
            "corr-1",
            "msg-1");

    private static DispatchNotificationCommand CreateSignupCommand()
    {
        var payload = JsonSerializer.Serialize(new CompanySignupInviteEmailEvent(
            1, 2, Guid.NewGuid(), Guid.NewGuid(), "user@example.com",
            CommunicationTemplateCodes.CompanyAdminInvitation, "User", "FGS", "https://invite", "72", "support@fgs.example"));

        return new DispatchNotificationCommand(new DispatchNotificationRequest
        {
            RoutingKey = IntegrationEventRoutingKeys.CompanySignupInviteEmail,
            Payload = payload,
            CorrelationId = "corr-1",
            MessageId = "msg-1"
        });
    }

    private DispatchNotificationCommandHandler CreateHandler() =>
        new(
            new NotificationDispatchRequestResolver(_mapper.Object),
            _idempotency.Object,
            _dispatcher.Object);
}
