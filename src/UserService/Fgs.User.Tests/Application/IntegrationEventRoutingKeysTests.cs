using Fgs.User.Application.IntegrationEvents;

namespace Fgs.User.Tests.Application;

public sealed class IntegrationEventRoutingKeysTests
{
      [Fact]
    public void ForEventType_UsesDefaultPrefix()
    {
        IntegrationEventRoutingKeys.ForEventType(IntegrationEventTypes.CompanySignupInviteEmail)
            .Should().Be(IntegrationEventRoutingKeys.CompanySignupInviteEmail);
    }

    [Fact]
    public void ForEventType_UsesCustomPrefix()
    {
        IntegrationEventRoutingKeys.ForEventType("TestEvent", "custom.")
            .Should().Be("custom.TestEvent");
    }
}
