using Fgs.Notification.Domain.Notifications;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Application.Notifications.History;
using Fgs.Notification.Application.Notifications.Providers;
using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Enums;
using Fgs.Notification.Infrastructure.Notifications.Channels;
using FluentAssertions;
using Moq;

namespace Fgs.Notification.Tests.Notifications;

public sealed class NotificationDispatcherTests
{
    [Fact]
    public async Task DispatchAsync_Email_DelegatesToResolvedProvider()
    {
        var emailProvider = new Mock<IEmailProvider>();
        emailProvider.Setup(p => p.ProviderName).Returns("SendGrid");
        emailProvider.Setup(p => p.SendAsync(It.IsAny<EmailNotificationMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NotificationDispatchResult(true, "msg-1", null));

        var factory = new Mock<INotificationProviderFactory>();
        factory.Setup(f => f.ResolveEmailProvider(It.IsAny<long>())).Returns(emailProvider.Object);

        var history = new Mock<INotificationHistoryRepository>();
        history.Setup(h => h.AddEmailAsync(It.IsAny<FgsEmailHistory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1L);
        history.Setup(h => h.UpdateEmailStatusAsync(
                It.IsAny<long>(),
                It.IsAny<NotificationStatus>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var renderer = new Mock<INotificationTemplateRenderer>();
        renderer.Setup(r => r.RenderAsync(
                It.IsAny<long>(),
                It.IsAny<long?>(),
                It.IsAny<NotificationChannel>(),
                It.IsAny<string>(),
                It.IsAny<IReadOnlyDictionary<string, string>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RenderedNotificationTemplate("Subject", "<p>Hi</p>", "Hi"));

        var dispatcher = new NotificationDispatcher(
            factory.Object,
            renderer.Object,
            history.Object,
            Mock.Of<Microsoft.Extensions.Logging.ILogger<NotificationDispatcher>>());

        var tenantId = 5001L;
        var result = await dispatcher.DispatchAsync(
            new NotificationDispatchRequest(
                tenantId,
                CompanyId: null,
                NotificationChannel.Email,
                "USER_INVITED",
                "user@example.com",
                new Dictionary<string, string> { ["DisplayName"] = "Test" },
                "corr-1",
                "msg-id-1"));

        result.Success.Should().BeTrue();
        emailProvider.Verify(
            p => p.SendAsync(
                It.Is<EmailNotificationMessage>(m => m.ToAddress == "user@example.com" && m.TenantId == tenantId),
                It.IsAny<CancellationToken>()),
            Times.Once);
        history.Verify(h => h.AddEmailAsync(It.IsAny<FgsEmailHistory>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
