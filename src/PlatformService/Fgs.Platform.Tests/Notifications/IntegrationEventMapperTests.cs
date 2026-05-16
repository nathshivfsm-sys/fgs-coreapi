using System.Text.Json;
using Fgs.Platform.Application.IntegrationEvents;
using Fgs.Platform.Domain.Notifications;
using Fgs.Platform.Infrastructure.Notifications.Queues;
using FluentAssertions;

namespace Fgs.Platform.Tests.Notifications;

public sealed class IntegrationEventMapperTests
{
    private readonly IntegrationEventMapper _mapper = new();

    [Fact]
    public void CanMap_KnownUserServiceRoutingKeys()
    {
        _mapper.CanMap(IntegrationEventRoutingKeys.CompanySignupInviteEmail).Should().BeTrue();
        _mapper.CanMap(IntegrationEventRoutingKeys.UserInvited).Should().BeTrue();
        _mapper.CanMap(IntegrationEventRoutingKeys.PasswordReset).Should().BeTrue();
        _mapper.CanMap(IntegrationEventRoutingKeys.CompanyCreated).Should().BeTrue();
    }

    [Fact]
    public void Map_CompanySignupInviteEmail_BuildsEmailDispatchRequest()
    {
        var tenantId = Guid.NewGuid();
        var payload = JsonSerializer.Serialize(new CompanySignupInviteEmailEvent(
            tenantId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "invite@example.com",
            "Alex",
            "https://example.com/invite"));

        var request = _mapper.Map(
            IntegrationEventRoutingKeys.CompanySignupInviteEmail,
            payload,
            "corr",
            "mid");

        request.Should().NotBeNull();
        request!.TenantId.Should().Be(tenantId);
        request.Channel.Should().Be(NotificationChannel.Email);
        request.Recipient.Should().Be("invite@example.com");
        request.TemplateName.Should().Be("CompanySignupInviteEmail");
    }
}
