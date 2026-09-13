using Fgs.Messaging.Models;
using Fgs.Messaging.Outbox;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Messaging.Tests;

public sealed class CompositeOutboxStoreTests
{
    [Fact]
    public async Task ClaimPendingBatchAsync_MergesSourcesOrderedByCreatedOn()
    {
        var older = DateTimeOffset.UtcNow.AddMinutes(-5);
        var newer = DateTimeOffset.UtcNow;

        var tenantSource = new Mock<ISchemaOutboxSource>();
        tenantSource.Setup(s => s.SourceKey).Returns("tenant");
        tenantSource
            .Setup(s => s.ClaimPendingBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                new ClaimedOutboxRow(CreateMessage("tenant", 2), newer)
            ]);

        var setupSource = new Mock<ISchemaOutboxSource>();
        setupSource.Setup(s => s.SourceKey).Returns("glo");
        setupSource
            .Setup(s => s.ClaimPendingBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                new ClaimedOutboxRow(CreateMessage("glo", 1), older)
            ]);

        var store = new CompositeOutboxStore(
            [tenantSource.Object, setupSource.Object],
            NullLogger<CompositeOutboxStore>.Instance);

        var messages = await store.ClaimPendingBatchAsync(20, CancellationToken.None);

        messages.Should().HaveCount(2);
        messages[0].SourceKey.Should().Be("glo");
        messages[1].SourceKey.Should().Be("tenant");
    }

    [Fact]
    public async Task ClaimPendingBatchAsync_DoesNotDropClaimedRows_WhenSourcesExceedBatchSize()
    {
        var older = DateTimeOffset.UtcNow.AddMinutes(-5);
        var newer = DateTimeOffset.UtcNow;

        var first = new Mock<ISchemaOutboxSource>();
        first.Setup(s => s.SourceKey).Returns("glo");
        first
            .Setup(s => s.ClaimPendingBatchAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                new ClaimedOutboxRow(CreateMessage("glo", 1), older)
            ]);

        var second = new Mock<ISchemaOutboxSource>();
        second.Setup(s => s.SourceKey).Returns("setup");
        // Must not be claimed when remaining budget is already exhausted after first source.
        second
            .Setup(s => s.ClaimPendingBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                new ClaimedOutboxRow(CreateMessage("setup", 2), newer)
            ]);

        var store = new CompositeOutboxStore(
            [first.Object, second.Object],
            NullLogger<CompositeOutboxStore>.Instance);

        var messages = await store.ClaimPendingBatchAsync(1, CancellationToken.None);

        messages.Should().HaveCount(1);
        messages[0].SourceKey.Should().Be("glo");
        second.Verify(
            s => s.ClaimPendingBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ClaimPendingBatchAsync_FillsRemainingFromOtherSources()
    {
        var empty = new Mock<ISchemaOutboxSource>();
        empty.Setup(s => s.SourceKey).Returns("glo");
        empty
            .Setup(s => s.ClaimPendingBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var rich = new Mock<ISchemaOutboxSource>();
        rich.Setup(s => s.SourceKey).Returns("setup");
        // First fair pass asks for 1 (batchSize 2 / 2 sources); returns full take → eligible for fill.
        rich
            .SetupSequence(s => s.ClaimPendingBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                new ClaimedOutboxRow(CreateMessage("setup", 1), DateTimeOffset.UtcNow)
            ])
            .ReturnsAsync([
                new ClaimedOutboxRow(CreateMessage("setup", 2), DateTimeOffset.UtcNow.AddSeconds(1))
            ]);

        var store = new CompositeOutboxStore(
            [empty.Object, rich.Object],
            NullLogger<CompositeOutboxStore>.Instance);

        var messages = await store.ClaimPendingBatchAsync(2, CancellationToken.None);

        messages.Should().HaveCount(2);
        messages.Select(m => m.Id).Should().BeEquivalentTo([1L, 2L]);
        rich.Verify(
            s => s.ClaimPendingBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2));
        empty.Verify(
            s => s.ClaimPendingBatchAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task MarkPublishedAsync_RoutesToMatchingSource()
    {
        var tenantSource = new Mock<ISchemaOutboxSource>();
        tenantSource.Setup(s => s.SourceKey).Returns("tenant");

        var store = new CompositeOutboxStore(
            [tenantSource.Object],
            NullLogger<CompositeOutboxStore>.Instance);

        await store.MarkPublishedAsync("tenant", 42, DateTimeOffset.UtcNow, CancellationToken.None);

        tenantSource.Verify(
            s => s.MarkPublishedAsync(42, It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HeartbeatProcessingAsync_RoutesToMatchingSource()
    {
        var tenantSource = new Mock<ISchemaOutboxSource>();
        tenantSource.Setup(s => s.SourceKey).Returns("tenant");

        var store = new CompositeOutboxStore(
            [tenantSource.Object],
            NullLogger<CompositeOutboxStore>.Instance);

        var updatedOn = DateTimeOffset.UtcNow;
        await store.HeartbeatProcessingAsync("tenant", 7, updatedOn, CancellationToken.None);

        tenantSource.Verify(
            s => s.HeartbeatProcessingAsync(7, updatedOn, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static PendingOutboxMessage CreateMessage(string sourceKey, long id) =>
        new(sourceKey, id, "TestEvent", "{}", Guid.NewGuid(), "exchange", "routing.key", 0, 5);
}
