using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Models;
using Fgs.Messaging.Options;
using Fgs.Messaging.Outbox;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace Fgs.Messaging.Tests;

public sealed class ContractOutboxDestinationResolverTests
{
    [Fact]
    public void Resolve_UsesPlatformEventsForCompanySignupInviteEmail()
    {
        var resolver = CreateResolver();

        var destination = resolver.Resolve(new PendingOutboxMessage(
            "tenant",
            1,
            IntegrationEventTypes.CompanySignupInviteEmail,
            "{}",
            Guid.NewGuid(),
            "fgs.user",
            "custom.routing",
            0,
            5));

        destination.DestinationName.Should().Be(IntegrationEventExchanges.UserEvents);
        destination.RoutingKey.Should().Be("custom.routing");
    }

    [Fact]
    public void Resolve_UsesTenantEventsForProvisionRequested()
    {
        var resolver = CreateResolver();

        var destination = resolver.Resolve(new PendingOutboxMessage(
            "tenant",
            1,
            IntegrationEventTypes.TenantProvisionRequested,
            "{}",
            Guid.NewGuid(),
            null,
            null,
            0,
            5));

        destination.DestinationName.Should().Be(IntegrationEventExchanges.TenantEvents);
    }

    [Fact]
    public void Resolve_FallsBackToEventTypeRoutingKey()
    {
        var resolver = CreateResolver();

        var destination = resolver.Resolve(new PendingOutboxMessage(
            "tenant",
            1,
            IntegrationEventTypes.CompanySignupInviteEmail,
            "{}",
            Guid.NewGuid(),
            null,
            null,
            0,
            5));

        destination.RoutingKey.Should().Be(IntegrationEventRoutingKeys.CompanySignupInviteEmail);
    }

    [Fact]
    public void Resolve_UsesInventoryEventsForInventoryStockChanged()
    {
        var resolver = CreateResolver();

        var destination = resolver.Resolve(new PendingOutboxMessage(
            "inventory",
            1,
            IntegrationEventTypes.InventoryStockChanged,
            "{}",
            Guid.NewGuid(),
            null,
            null,
            0,
            5));

        destination.DestinationName.Should().Be(IntegrationEventExchanges.InventoryEvents);
    }

    [Fact]
    public void Resolve_UsesInventoryEventsForPurchaseOrderStatusChanged()
    {
        var resolver = CreateResolver();

        var destination = resolver.Resolve(new PendingOutboxMessage(
            "inventory",
            1,
            IntegrationEventTypes.PurchaseOrderStatusChanged,
            "{}",
            Guid.NewGuid(),
            null,
            null,
            0,
            5));

        destination.DestinationName.Should().Be(IntegrationEventExchanges.InventoryEvents);
    }

    private static ContractOutboxDestinationResolver CreateResolver() =>
        new(MsOptions.Create(new RabbitMqOptions
        {
            ExchangeName = IntegrationEventExchanges.UserEvents,
            RoutingKeyPrefix = "user."
        }));
}
