using Fgs.Messaging.Models;
using Fgs.Publisher.Infrastructure.Outbox;
using Moq;

namespace Fgs.Publisher.Tests;

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

        var store = new CompositeOutboxStore([tenantSource.Object, setupSource.Object]);

        var messages = await store.ClaimPendingBatchAsync(20, CancellationToken.None);

        messages.Should().HaveCount(2);
        messages[0].SourceKey.Should().Be("glo");
        messages[1].SourceKey.Should().Be("tenant");
    }

    [Fact]
    public async Task MarkPublishedAsync_RoutesToMatchingSource()
    {
        var tenantSource = new Mock<ISchemaOutboxSource>();
        tenantSource.Setup(s => s.SourceKey).Returns("tenant");

        var store = new CompositeOutboxStore([tenantSource.Object]);

        await store.MarkPublishedAsync("tenant", 42, DateTimeOffset.UtcNow, CancellationToken.None);

        tenantSource.Verify(
            s => s.MarkPublishedAsync(42, It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static PendingOutboxMessage CreateMessage(string sourceKey, long id) =>
        new(sourceKey, id, "TestEvent", "{}", Guid.NewGuid(), "exchange", "routing.key", 0, 5);
}
