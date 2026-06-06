using Fgs.Contracts.IntegrationEvents;

namespace Fgs.User.Tests.Application;

public sealed class IntegrationEventExchangesTests
{
    [Theory]
    [InlineData(IntegrationEventTypes.TenantProvisionRequested, IntegrationEventExchanges.TenantEvents)]
    [InlineData(IntegrationEventTypes.TenantProvisionCompleted, IntegrationEventExchanges.TenantEvents)]
    [InlineData(IntegrationEventTypes.CompanySignupInviteEmail, IntegrationEventExchanges.UserEvents)]
    public void ForEventType_ReturnsExpectedExchange(string eventType, string expectedExchange) =>
        IntegrationEventExchanges.ForEventType(eventType).Should().Be(expectedExchange);
}
