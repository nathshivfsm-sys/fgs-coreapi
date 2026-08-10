using Fgs.Messaging.Consumer;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Fgs.Messaging.Tests;

public sealed class DistributedCacheConsumerIdempotencyStoreTests
{
    [Fact]
    public async Task TryMarkProcessedAsync_FirstCall_ReturnsTrue_SecondCall_ReturnsFalse()
    {
        var cache = new MemoryDistributedCache(
            Microsoft.Extensions.Options.Options.Create(new MemoryDistributedCacheOptions()));
        var store = new DistributedCacheConsumerIdempotencyStore(
            cache,
            NullLogger<DistributedCacheConsumerIdempotencyStore>.Instance);

        var first = await store.TryMarkProcessedAsync("msg-1", "tenant.provision.requested");
        var second = await store.TryMarkProcessedAsync("msg-1", "tenant.provision.requested");

        first.Should().BeTrue();
        second.Should().BeFalse();
    }

    [Fact]
    public async Task TryMarkProcessedAsync_EmptyMessageId_ReturnsTrue()
    {
        var cache = new MemoryDistributedCache(
            Microsoft.Extensions.Options.Options.Create(new MemoryDistributedCacheOptions()));
        var store = new DistributedCacheConsumerIdempotencyStore(
            cache,
            NullLogger<DistributedCacheConsumerIdempotencyStore>.Instance);

        var result = await store.TryMarkProcessedAsync(" ", "tenant.provision.requested");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task HasBeenProcessedAsync_FalseUntilMarked()
    {
        var cache = new MemoryDistributedCache(
            Microsoft.Extensions.Options.Options.Create(new MemoryDistributedCacheOptions()));
        var store = new DistributedCacheConsumerIdempotencyStore(
            cache,
            NullLogger<DistributedCacheConsumerIdempotencyStore>.Instance);

        (await store.HasBeenProcessedAsync("msg-1", "tenant.provision.requested")).Should().BeFalse();
        await store.TryMarkProcessedAsync("msg-1", "tenant.provision.requested");
        (await store.HasBeenProcessedAsync("msg-1", "tenant.provision.requested")).Should().BeTrue();
    }
}
