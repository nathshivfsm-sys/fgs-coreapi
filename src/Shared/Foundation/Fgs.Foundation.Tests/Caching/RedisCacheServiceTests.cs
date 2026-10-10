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
    private readonly Mock<IDatabase> _database = new();
    private readonly RedisCacheService _sut;

    public RedisCacheServiceTests()
    {
        _database
            .Setup(d => d.SetAddAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);
        _database
            .Setup(d => d.SetMembersAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync([]);
        _database
            .Setup(d => d.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);
        _database
            .Setup(d => d.KeyDeleteAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(0);
        _connectionMultiplexer
            .Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_database.Object);

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
    public async Task RemoveAsync_WhenRedisThrows_Rethrows()
    {
        _distributedCache
            .Setup(c => c.RemoveAsync("key", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new RedisConnectionException(ConnectionFailureType.UnableToConnect, "down"));

        var act = async () => await _sut.RemoveAsync("key");

        await act.Should().ThrowAsync<RedisConnectionException>();
    }

    [Fact]
    public async Task SetAsync_IndexesTenantAndGlobalKeys()
    {
        await _sut.SetAsync("tenant:1:company:2:vehicles:9", new TestDto("van"));
        await _sut.SetAsync("global:countries:lookup:activeOnly=true", new TestDto("us"));
        await _sut.SetAsync("login:pkce:state", new TestDto("pkce"));

        _database.Verify(
            d => d.SetAddAsync(
                (RedisKey)"fgs:tenant:1:company:2:vehicles:",
                (RedisValue)"fgs:tenant:1:company:2:vehicles:9",
                It.IsAny<CommandFlags>()),
            Times.Once);
        _database.Verify(
            d => d.SetAddAsync(
                (RedisKey)"fgs:global:countries:",
                (RedisValue)"fgs:global:countries:lookup:activeOnly=true",
                It.IsAny<CommandFlags>()),
            Times.Once);
        _database.Verify(
            d => d.SetAddAsync(
                It.Is<RedisKey>(key => key.ToString().Contains("login:pkce", StringComparison.Ordinal)),
                It.IsAny<RedisValue>(),
                It.IsAny<CommandFlags>()),
            Times.Never);
    }

    [Fact]
    public async Task RemoveByPrefixAsync_DeletesIndexedKeysWithoutScanning()
    {
        _database
            .Setup(d => d.SetMembersAsync((RedisKey)"fgs:tenant:1:company:2:vehicles:", It.IsAny<CommandFlags>()))
            .ReturnsAsync([(RedisValue)"fgs:tenant:1:company:2:vehicles:9"]);
        _database
            .Setup(d => d.KeyDeleteAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(1);

        await _sut.RemoveByPrefixAsync("tenant:1:company:2:vehicles:");

        _database.Verify(
            d => d.KeyDeleteAsync(
                It.Is<RedisKey[]>(keys => keys.Length == 1 && keys[0] == (RedisKey)"fgs:tenant:1:company:2:vehicles:9"),
                It.IsAny<CommandFlags>()),
            Times.Once);
        _database.Verify(
            d => d.KeyDeleteAsync((RedisKey)"fgs:tenant:1:company:2:vehicles:", It.IsAny<CommandFlags>()),
            Times.Once);
        _connectionMultiplexer.Verify(m => m.GetEndPoints(It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task RemoveByPrefixAsync_WhenRedisThrows_Rethrows()
    {
        _database
            .Setup(d => d.SetMembersAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ThrowsAsync(new RedisConnectionException(ConnectionFailureType.UnableToConnect, "down"));

        var act = async () => await _sut.RemoveByPrefixAsync("tenant:1:company:2:vehicles:");

        await act.Should().ThrowAsync<RedisConnectionException>();
    }

    [Fact]
    public async Task GetOrSetAsync_ConcurrentCalls_RunFactoryOnce()
    {
        var cache = new MemoryDistributedCacheFake();
        var sut = new RedisCacheService(
            cache,
            _connectionMultiplexer.Object,
            Options.Create(new RedisCacheOptions
            {
                InstanceName = "fgs:",
                DefaultAbsoluteExpirationMinutes = 30
            }),
            NullLogger<RedisCacheService>.Instance);

        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var calls = 0;

        var first = sut.GetOrSetAsync("tenant:1:company:2:vehicles:1", async () =>
        {
            Interlocked.Increment(ref calls);
            started.TrySetResult();
            await release.Task;
            return new TestDto("fresh");
        });

        await started.Task;
        var second = sut.GetOrSetAsync(
            "tenant:1:company:2:vehicles:1",
            () =>
            {
                Interlocked.Increment(ref calls);
                return Task.FromResult(new TestDto("other"));
            });

        release.TrySetResult();
        var results = await Task.WhenAll(first, second);

        calls.Should().Be(1);
        results.Should().AllBeEquivalentTo(new TestDto("fresh"));
    }

    [Fact]
    public async Task GetAndRemoveAsync_ReturnsDeserializedValue()
    {
        var dto = new TestDto("pkce");
        _database
            .Setup(d => d.ScriptEvaluateAsync(
                It.IsAny<string>(),
                It.IsAny<RedisKey[]>(),
                It.IsAny<RedisValue[]>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisResult.Create((RedisValue)CacheJsonSerializer.Serialize(dto)));

        var result = await _sut.GetAndRemoveAsync<TestDto>("login:pkce:state");

        result.Should().BeEquivalentTo(dto);
        _database.Verify(
            d => d.ScriptEvaluateAsync(
                It.IsAny<string>(),
                It.Is<RedisKey[]>(keys => keys.Length == 1 && keys[0] == (RedisKey)"fgs:login:pkce:state"),
                It.IsAny<RedisValue[]>(),
                It.IsAny<CommandFlags>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAndRemoveAsync_WhenMissing_ReturnsNull()
    {
        _database
            .Setup(d => d.ScriptEvaluateAsync(
                It.IsAny<string>(),
                It.IsAny<RedisKey[]>(),
                It.IsAny<RedisValue[]>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisResult.Create(RedisValue.Null));

        var result = await _sut.GetAndRemoveAsync<TestDto>("login:pkce:missing");

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAndRemoveAsync_WhenRedisThrows_Rethrows()
    {
        _database
            .Setup(d => d.ScriptEvaluateAsync(
                It.IsAny<string>(),
                It.IsAny<RedisKey[]>(),
                It.IsAny<RedisValue[]>(),
                It.IsAny<CommandFlags>()))
            .ThrowsAsync(new RedisConnectionException(ConnectionFailureType.UnableToConnect, "down"));

        var act = async () => await _sut.GetAndRemoveAsync<TestDto>("login:pkce:state");

        await act.Should().ThrowAsync<RedisConnectionException>();
    }

    private sealed record TestDto(string Name);

    private sealed class MemoryDistributedCacheFake : IDistributedCache
    {
        private readonly Dictionary<string, byte[]> _store = new(StringComparer.Ordinal);

        public byte[]? Get(string key) => GetAsync(key).GetAwaiter().GetResult();

        public Task<byte[]?> GetAsync(string key, CancellationToken token = default)
        {
            lock (_store)
            {
                return Task.FromResult(_store.TryGetValue(key, out var value) ? value : null);
            }
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options) =>
            SetAsync(key, value, options).GetAwaiter().GetResult();

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            lock (_store)
            {
                _store[key] = value;
            }

            return Task.CompletedTask;
        }

        public void Refresh(string key)
        {
        }

        public Task RefreshAsync(string key, CancellationToken token = default) => Task.CompletedTask;

        public void Remove(string key) => RemoveAsync(key).GetAwaiter().GetResult();

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            lock (_store)
            {
                _store.Remove(key);
            }

            return Task.CompletedTask;
        }
    }
}
