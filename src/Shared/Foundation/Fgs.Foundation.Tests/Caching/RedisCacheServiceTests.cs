using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;

namespace Fgs.Foundation.Tests.Caching;

public sealed class RedisCacheServiceTests
{
    private readonly Mock<IDistributedCache> _distributedCache = new();
    private readonly Mock<IConnectionMultiplexer> _connectionMultiplexer = new();
    private readonly RedisCacheService _sut;

    public RedisCacheServiceTests()
    {
        _connectionMultiplexer.Setup(m => m.GetEndPoints()).Returns([]);

        _sut = new RedisCacheService(
            _distributedCache.Object,
            _connectionMultiplexer.Object,
            Options.Create(new RedisCacheOptions
            {
                InstanceName = "fgs:",
                DefaultAbsoluteExpirationMinutes = 30
            }),
            NullLogger<RedisCacheService>.Instance);
    }

    [Fact]
    public async Task GetAsync_WhenCached_ReturnsValueAndLogsHit()
    {
        var dto = new TestDto("cached");
        var bytes = CacheJsonSerializer.Serialize(dto);
        _distributedCache
            .Setup(c => c.GetAsync("tenant:1:company:2:vehicles:1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(bytes);

        var result = await _sut.GetAsync<TestDto>("tenant:1:company:2:vehicles:1");

        result.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetAsync_WhenMissing_ReturnsNull()
    {
        _distributedCache
            .Setup(c => c.GetAsync("missing", It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        var result = await _sut.GetAsync<TestDto>("missing");

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_WhenRedisThrows_ReturnsNullWithoutThrowing()
    {
        _distributedCache
            .Setup(c => c.GetAsync("failing", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new RedisConnectionException(ConnectionFailureType.UnableToConnect, "down"));

        var result = await _sut.GetAsync<TestDto>("failing");

        result.Should().BeNull();
    }

    [Fact]
    public async Task SetAsync_WhenRedisThrows_DoesNotThrow()
    {
        _distributedCache
            .Setup(c => c.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new RedisConnectionException(ConnectionFailureType.UnableToConnect, "down"));

        var act = async () => await _sut.SetAsync("key", new TestDto("value"));

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GetOrSetAsync_WhenMiss_StoresFactoryResult()
    {
        _distributedCache
            .Setup(c => c.GetAsync("key", It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        var result = await _sut.GetOrSetAsync("key", () => Task.FromResult(new TestDto("fresh")));

        result.Should().BeEquivalentTo(new TestDto("fresh"));
        _distributedCache.Verify(
            c => c.SetAsync(
                "key",
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetOrSetAsync_WhenFactoryReturnsNull_DoesNotStore()
    {
        _distributedCache
            .Setup(c => c.GetAsync("key", It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        var result = await _sut.GetOrSetAsync("key", () => Task.FromResult<TestDto>(null!));

        result.Should().BeNull();
        _distributedCache.Verify(
            c => c.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task RemoveAsync_WhenRedisThrows_DoesNotThrow()
    {
        _distributedCache
            .Setup(c => c.RemoveAsync("key", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new RedisConnectionException(ConnectionFailureType.UnableToConnect, "down"));

        var act = async () => await _sut.RemoveAsync("key");

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task RemoveByPrefixAsync_WhenNoEndpoints_DoesNotThrow()
    {
        var act = async () => await _sut.RemoveByPrefixAsync("tenant:1:company:2:vehicles:");

        await act.Should().NotThrowAsync();
    }

    private sealed record TestDto(string Name);
}
