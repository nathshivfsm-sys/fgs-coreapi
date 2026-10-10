using System.Collections.Concurrent;
using System.Diagnostics;
using Fgs.Contracts.Observability;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Caching.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Fgs.Foundation.Caching;

public sealed class RedisCacheService : ICacheService
{
    /// <summary>
    /// IDistributedCache stores a Redis hash (field <c>data</c>). GETDEL only reads strings and
    /// returns WRONGTYPE for those entries, so PKCE consume uses this atomic hash read-and-delete.
    /// </summary>
    private const string GetAndDeleteDataScript = """
        local data = redis.call('HGET', KEYS[1], 'data')
        if data then
          redis.call('DEL', KEYS[1])
        end
        return data
        """;

    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly RedisCacheOptions _options;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly IFgsMetrics _metrics;
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _getOrSetGates = new(StringComparer.Ordinal);

    public RedisCacheService(
        IDistributedCache distributedCache,
        IConnectionMultiplexer connectionMultiplexer,
        IOptions<RedisCacheOptions> options,
        ILogger<RedisCacheService> logger,
        IFgsMetrics? metrics = null)
    {
        _distributedCache = distributedCache;
        _connectionMultiplexer = connectionMultiplexer;
        _options = options.Value;
        _logger = logger;
        _metrics = metrics ?? NoOpFgsMetrics.Instance;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var data = await _distributedCache.GetAsync(key, cancellationToken);
            if (data is null || data.Length == 0)
            {
                _metrics.Increment("cache.miss");
                _metrics.Histogram("cache.latency_ms", sw.Elapsed.TotalMilliseconds, ("result", "miss"));
                _logger.LogInformation("CacheMiss {CacheKey}", key);
                return null;
            }

            var value = CacheJsonSerializer.Deserialize<T>(data);
            if (value is null)
            {
                _metrics.Increment("cache.miss");
                _metrics.Histogram("cache.latency_ms", sw.Elapsed.TotalMilliseconds, ("result", "miss"));
                _logger.LogInformation("CacheMiss {CacheKey}", key);
                return null;
            }

            _metrics.Increment("cache.hit");
            _metrics.Histogram("cache.latency_ms", sw.Elapsed.TotalMilliseconds, ("result", "hit"));
            _logger.LogInformation("CacheHit {CacheKey}", key);
            return value;
        }
        catch (Exception ex)
        {
            _metrics.Increment("cache.error", tags: ("operation", nameof(GetAsync)));
            _logger.LogWarning(ex, "RedisFailure {Operation} {CacheKey}", nameof(GetAsync), key);
            return null;
        }
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var expiration = absoluteExpiration ?? TimeSpan.FromMinutes(_options.DefaultAbsoluteExpirationMinutes);
            var data = CacheJsonSerializer.Serialize(value);
            await _distributedCache.SetAsync(
                key,
                data,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration
                },
                cancellationToken);

            await IndexKeyAsync(key);

            _logger.LogInformation(
                "CacheSet {CacheKey} {ExpirationMinutes}",
                key,
                expiration.TotalMinutes);
        }
        catch (Exception ex)
        {
            _metrics.Increment("cache.error", tags: ("operation", nameof(SetAsync)));
            _logger.LogWarning(ex, "RedisFailure {Operation} {CacheKey}", nameof(SetAsync), key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            _logger.LogInformation("CacheRemove {CacheKey}", key);
        }
        catch (Exception ex)
        {
            _metrics.Increment("cache.error", tags: ("operation", nameof(RemoveAsync)));
            _logger.LogWarning(ex, "RedisFailure {Operation} {CacheKey}", nameof(RemoveAsync), key);
            throw;
        }
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            var database = _connectionMultiplexer.GetDatabase();
            var indexKey = PhysicalKey(prefix);
            var members = await database.SetMembersAsync(indexKey);
            var totalRemoved = 0;

            if (members.Length > 0)
            {
                var keys = new RedisKey[members.Length];
                for (var i = 0; i < members.Length; i++)
                {
                    keys[i] = (RedisKey)members[i].ToString();
                }

                totalRemoved = (int)await database.KeyDeleteAsync(keys);
            }

            await database.KeyDeleteAsync(indexKey);
            _logger.LogInformation("CacheRemove {CachePrefix} {RemovedCount}", prefix, totalRemoved);
        }
        catch (Exception ex)
        {
            _metrics.Increment("cache.error", tags: ("operation", nameof(RemoveByPrefixAsync)));
            _logger.LogWarning(ex, "RedisFailure {Operation} {CachePrefix}", nameof(RemoveByPrefixAsync), prefix);
            throw;
        }
    }

    public async Task<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default) where T : class
    {
        var cached = await GetAsync<T>(key, cancellationToken);
        if (cached is not null)
        {
            return cached;
        }

        var gate = _getOrSetGates.GetOrAdd(key, static _ => new SemaphoreSlim(1, 1));
        await gate.WaitAsync(cancellationToken);
        try
        {
            cached = await GetAsync<T>(key, cancellationToken);
            if (cached is not null)
            {
                return cached;
            }

            var value = await factory();
            if (value is not null)
            {
                await SetAsync(key, value, absoluteExpiration, cancellationToken);
            }

            return value;
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<T?> GetAndRemoveAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            var database = _connectionMultiplexer.GetDatabase();
            var result = await database.ScriptEvaluateAsync(
                GetAndDeleteDataScript,
                [PhysicalKey(key)],
                Array.Empty<RedisValue>());

            if (result.IsNull)
            {
                return null;
            }

            var data = (byte[]?)result;
            if (data is null || data.Length == 0)
            {
                return null;
            }

            return CacheJsonSerializer.Deserialize<T>(data);
        }
        catch (Exception ex)
        {
            _metrics.Increment("cache.error", tags: ("operation", nameof(GetAndRemoveAsync)));
            _logger.LogWarning(ex, "RedisFailure {Operation} {CacheKey}", nameof(GetAndRemoveAsync), key);
            throw;
        }
    }

    private async Task IndexKeyAsync(string key)
    {
        var prefix = TryGetIndexPrefix(key);
        if (prefix is null)
        {
            return;
        }

        var database = _connectionMultiplexer.GetDatabase();
        await database.SetAddAsync(PhysicalKey(prefix), (RedisValue)PhysicalKey(key).ToString());
    }

    private RedisKey PhysicalKey(string logicalKey) => $"{_options.InstanceName}{logicalKey}";

    /// <summary>
    /// Prefix for <c>tenant:{tenantId}:company:{companyId}:{entity}:</c> or <c>global:{entity}:</c>.
    /// </summary>
    internal static string? TryGetIndexPrefix(string key)
    {
        if (key.StartsWith("tenant:", StringComparison.Ordinal))
        {
            var tenantEnd = key.IndexOf(':', "tenant:".Length);
            if (tenantEnd < 0)
            {
                return null;
            }

            const string companyMarker = ":company:";
            if (!key.AsSpan(tenantEnd, Math.Min(companyMarker.Length, key.Length - tenantEnd))
                    .SequenceEqual(companyMarker))
            {
                return null;
            }

            var companyIdStart = tenantEnd + companyMarker.Length;
            var companyEnd = key.IndexOf(':', companyIdStart);
            if (companyEnd < 0 || companyEnd == companyIdStart)
            {
                return null;
            }

            var entityEnd = key.IndexOf(':', companyEnd + 1);
            if (entityEnd < 0 || entityEnd == companyEnd + 1 || entityEnd >= key.Length - 1)
            {
                return null;
            }

            return key[..(entityEnd + 1)];
        }

        if (key.StartsWith("global:", StringComparison.Ordinal))
        {
            var entityEnd = key.IndexOf(':', "global:".Length);
            if (entityEnd < 0 || entityEnd == "global:".Length || entityEnd >= key.Length - 1)
            {
                return null;
            }

            return key[..(entityEnd + 1)];
        }

        return null;
    }
}
