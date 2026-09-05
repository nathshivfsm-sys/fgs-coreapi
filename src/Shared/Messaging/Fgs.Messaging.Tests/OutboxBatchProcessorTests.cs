using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Models;
using Fgs.Messaging.Options;
using Fgs.Messaging.Outbox;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace Fgs.Messaging.Tests;

public sealed class OutboxBatchProcessorTests
{
    [Fact]
    public async Task ProcessBatchAsync_MarksPublishedOnlyAfterSuccessfulPublish()
    {
        var message = new PendingOutboxMessage(
            "tenant",
            10,
            "TestEvent",
            "{}",
            Guid.NewGuid(),
            null,
            null,
            0,
            5);

        var store = new Mock<IOutboxStore>();
        store
            .Setup(s => s.ClaimPendingBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([message]);

        var publisher = new Mock<IIntegrationEventPublisher>();
        publisher
            .Setup(p => p.PublishAsync(
                It.IsAny<IntegrationEventDestination>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var resolver = new Mock<IOutboxDestinationResolver>();
        resolver
            .Setup(r => r.Resolve(message))
            .Returns(new IntegrationEventDestination("test.events", "test.key"));

        var processor = new OutboxBatchProcessor(
            store.Object,
            publisher.Object,
            resolver.Object,
            MsOptions.Create(new OutboxOptions { BatchSize = 20, MaxRetryCount = 5 }),
            NullLogger<OutboxBatchProcessor>.Instance);

        await processor.ProcessBatchAsync(CancellationToken.None);

        publisher.Verify(
            p => p.PublishAsync(
                It.Is<IntegrationEventDestination>(d =>
                    d.DestinationName == "test.events" && d.RoutingKey == "test.key"),
                "{}",
                message.CorrelationId.ToString(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        store.Verify(
            s => s.MarkPublishedAsync("tenant", 10, It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ProcessBatchAsync_DoesNotMarkPublishedWhenPublishFails()
    {
        var message = new PendingOutboxMessage(
            "tenant",
            11,
            "TestEvent",
            "{}",
            Guid.NewGuid(),
            null,
            null,
            0,
            5);

        var store = new Mock<IOutboxStore>();
        store
            .Setup(s => s.ClaimPendingBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([message]);

        var publisher = new Mock<IIntegrationEventPublisher>();
        publisher
            .Setup(p => p.PublishAsync(
                It.IsAny<IntegrationEventDestination>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("broker down"));

        var resolver = new Mock<IOutboxDestinationResolver>();
        resolver
            .Setup(r => r.Resolve(message))
            .Returns(new IntegrationEventDestination("test.events", "test.key"));

        var processor = new OutboxBatchProcessor(
            store.Object,
            publisher.Object,
            resolver.Object,
            MsOptions.Create(new OutboxOptions { BatchSize = 20, MaxRetryCount = 5 }),
            NullLogger<OutboxBatchProcessor>.Instance);

        await processor.ProcessBatchAsync(CancellationToken.None);

        store.Verify(
            s => s.MarkPublishedAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()),
            Times.Never);

        store.Verify(
            s => s.MarkRetryOrFailedAsync(
                "tenant",
                11,
                1,
                It.IsAny<string>(),
                false,
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
