using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Models;
using Fgs.Messaging.Options;
using Fgs.Publisher.Infrastructure.Options;
using Fgs.Publisher.Infrastructure.Outbox;
using Microsoft.Extensions.Options;

namespace Fgs.Publisher.Tests;

public sealed class GlobalOutboxRoutingResolverTests
{
    [Fact]
    public void ResolveExchangeName_UsesRowValueWhenPresent()
    {
        var resolver = CreateResolver();

        var exchange = resolver.ResolveExchangeName(new PendingOutboxMessage(
            "tenant",
            1,
            IntegrationEventTypes.CompanySignupInviteEmail,
            "{}",
            Guid.NewGuid(),
            "custom.exchange",
            "custom.routing",
            0,
            5));

        exchange.Should().Be("custom.exchange");
    }

    [Fact]
    public void ResolveExchangeName_UsesTenantEventsForProvisionRequested()
    {
        var resolver = CreateResolver();

        var exchange = resolver.ResolveExchangeName(new PendingOutboxMessage(
            "tenant",
            1,
            IntegrationEventTypes.TenantProvisionRequested,
            "{}",
            Guid.NewGuid(),
            null,
            null,
            0,
            5));

        exchange.Should().Be("tenant.events");
    }

    [Fact]
    public void ResolveRoutingKey_FallsBackToEventTypePrefix()
    {
        var resolver = CreateResolver();

        var routingKey = resolver.ResolveRoutingKey(new PendingOutboxMessage(
            "tenant",
            1,
            IntegrationEventTypes.CompanySignupInviteEmail,
            "{}",
            Guid.NewGuid(),
            null,
            null,
            0,
            5));

        routingKey.Should().Be(IntegrationEventRoutingKeys.CompanySignupInviteEmail);
    }

    private static GlobalOutboxRoutingResolver CreateResolver() =>
        new(
            Options.Create(new RabbitMqOptions
            {
                ExchangeName = "fgs.user",
                RoutingKeyPrefix = "user."
            }),
            Options.Create(new TenantProvisioningOptions
            {
                TenantEventsExchangeName = "tenant.events"
            }));
}
