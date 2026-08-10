using Fgs.Messaging.Consumer;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StackExchange.Redis;

namespace Fgs.Messaging.Tests;

public sealed class RedisConsumerIdempotencyStoreTests
{
    [Fact]
    public async Task TryMarkProcessedAsync_WhenSetNxSucceeds_ReturnsTrue()
    {
        var database = new Mock<IDatabase>();
        database
            .Setup(d => d.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                When.NotExists))
            .ReturnsAsync(true);

        var multiplexer = new Mock<IConnectionMultiplexer>();
        multiplexer.Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(database.Object);

        var store = new RedisConsumerIdempotencyStore(
            multiplexer.Object,
            NullLogger<RedisConsumerIdempotencyStore>.Instance);

        var result = await store.TryMarkProcessedAsync("msg-1", "tenant.provision.requested");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task TryMarkProcessedAsync_WhenKeyExists_ReturnsFalse()
    {
        var database = new Mock<IDatabase>();
        database
            .Setup(d => d.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                When.NotExists))
            .ReturnsAsync(false);

        var multiplexer = new Mock<IConnectionMultiplexer>();
        multiplexer.Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(database.Object);

        var store = new RedisConsumerIdempotencyStore(
            multiplexer.Object,
            NullLogger<RedisConsumerIdempotencyStore>.Instance);

        var result = await store.TryMarkProcessedAsync("msg-1", "tenant.provision.requested");
        result.Should().BeFalse();
    }

    [Fact]
    public async Task TryMarkProcessedAsync_EmptyMessageId_ReturnsTrueWithoutRedis()
    {
        var multiplexer = new Mock<IConnectionMultiplexer>(MockBehavior.Strict);
        var store = new RedisConsumerIdempotencyStore(
            multiplexer.Object,
            NullLogger<RedisConsumerIdempotencyStore>.Instance);

        var result = await store.TryMarkProcessedAsync(" ", "rk");
        result.Should().BeTrue();
        multiplexer.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task HasBeenProcessedAsync_UsesKeyExists()
    {
        var database = new Mock<IDatabase>();
        database
            .Setup(d => d.KeyExistsAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        var multiplexer = new Mock<IConnectionMultiplexer>();
        multiplexer.Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(database.Object);

        var store = new RedisConsumerIdempotencyStore(
            multiplexer.Object,
            NullLogger<RedisConsumerIdempotencyStore>.Instance);

        var result = await store.HasBeenProcessedAsync("msg-1", "tenant.provision.requested");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task HasBeenProcessedAsync_EmptyMessageId_ReturnsFalseWithoutRedis()
    {
        var multiplexer = new Mock<IConnectionMultiplexer>(MockBehavior.Strict);
        var store = new RedisConsumerIdempotencyStore(
            multiplexer.Object,
            NullLogger<RedisConsumerIdempotencyStore>.Instance);

        var result = await store.HasBeenProcessedAsync(" ", "rk");
        result.Should().BeFalse();
        multiplexer.VerifyNoOtherCalls();
    }
}
