using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Requests;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Application.Notifications.Dispatch;
using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Domain.Notifications;
using Moq;

namespace Fgs.Notification.Tests.Notifications;

public sealed class NotificationDispatchRequestResolverTests
{
    private readonly Mock<IIntegrationEventMapper> _mapper = new();

    [Fact]
    public void Resolve_EventShape_ReturnsMappedDispatchRequest()
    {
        _mapper.Setup(m => m.CanMap(IntegrationEventRoutingKeys.CompanySignupInviteEmail)).Returns(true);
        var mapped = new NotificationDispatchRequest(
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
                "{}",
                "corr-1",
                "msg-1"))
            .Returns(mapped);

        var result = CreateResolver().Resolve(new DispatchNotificationRequest
        {
            RoutingKey = IntegrationEventRoutingKeys.CompanySignupInviteEmail,
            Payload = "{}",
            CorrelationId = "corr-1",
            MessageId = "msg-1"
        });

        result.IsFailure.Should().BeFalse();
        result.IsNoContent.Should().BeFalse();
        result.DispatchRequest.Should().BeSameAs(mapped);
        result.RequiresIdempotency.Should().BeTrue();
        result.IdempotencyKey.Should().Be(IntegrationEventRoutingKeys.CompanySignupInviteEmail);
    }

    [Fact]
    public void Resolve_ExplicitShape_ReturnsDirectDispatchRequest()
    {
        var result = CreateResolver().Resolve(new DispatchNotificationRequest
        {
            TenantId = 1,
            CompanyId = 2,
            Channel = "Email",
            TemplateCode = CommunicationTemplateCodes.CompanyAdminInvitation,
            Recipient = "user@example.com",
            Tokens = new Dictionary<string, string> { ["Name"] = "User" }
        });

        result.IsFailure.Should().BeFalse();
        result.DispatchRequest!.TenantId.Should().Be(1);
        result.DispatchRequest.CompanyId.Should().Be(2);
        result.DispatchRequest.Channel.Should().Be(NotificationChannel.Email);
        result.RequiresIdempotency.Should().BeFalse();
    }

    [Fact]
    public void Resolve_InvalidShape_ReturnsFailure()
    {
        var result = CreateResolver().Resolve(new DispatchNotificationRequest
        {
            RoutingKey = IntegrationEventRoutingKeys.CompanySignupInviteEmail
        });

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
    }

    private NotificationDispatchRequestResolver CreateResolver() =>
        new(_mapper.Object);
}
