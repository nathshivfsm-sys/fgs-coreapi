using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Models;
using Fgs.Messaging.RabbitMq;
using Moq;

namespace Fgs.Messaging.Tests;

public sealed class RabbitMqIntegrationEventPublisherTests
{
    [Fact]
    public async Task PublishAsync_MapsDestinationToExchangeAndRoutingKey()
    {
        var rabbit = new Mock<IRabbitMqPublisher>();
        rabbit
            .Setup(r => r.PublishAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var publisher = new RabbitMqIntegrationEventPublisher(rabbit.Object);
        var destination = new IntegrationEventDestination("setup.events", "credential.changed");

        await publisher.PublishAsync(destination, "{\"ok\":true}", "corr-1", CancellationToken.None);

        rabbit.Verify(
            r => r.PublishAsync(
                "setup.events",
                "credential.changed",
                "{\"ok\":true}",
                "corr-1",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
