using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Caching.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Fgs.Foundation.Caching;

public sealed class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly RedisCacheOptions _options;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(
        IDistributedCache distributedCache,
        IConnectionMultiplexer connectionMultiplexer,
        IOptions<RedisCacheOptions> options,
        ILogger<RedisCacheService> logger)
    {
        _distributedCache = distributedCache;
        _connectionMultiplexer = connectionMultiplexer;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var data = await _distributedCache.GetAsync(key, cancellationToken);
            if (data is null || data.Length == 0)
            {
                _logger.LogInformation("CacheMiss {CacheKey}", key);
                return null;
            }

            var value = CacheJsonSerializer.Deserialize<T>(data);
            if (value is null)
            {
                _logger.LogInformation("CacheMiss {CacheKey}", key);
                return null;
            }

            _logger.LogInformation("CacheHit {CacheKey}", key);
            return value;
        }
        catch (Exception ex)
        {
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

            _logger.LogInformation(
                "CacheSet {CacheKey} {ExpirationMinutes}",
                key,
                expiration.TotalMinutes);
        }
        catch (Exception ex)
        {
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
            _logger.LogWarning(ex, "RedisFailure {Operation} {CacheKey}", nameof(RemoveAsync), key);
        }
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        try
        {
            var redisPrefix = $"{_options.InstanceName}{prefix}";
            var endpoints = _connectionMultiplexer.GetEndPoints();
            if (endpoints.Length == 0)
            {
                return;
            }

            var database = _connectionMultiplexer.GetDatabase();
            var totalRemoved = 0;

            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                if (!server.IsConnected || server.IsReplica)
                {
                    continue;
                }

                await foreach (var key in server.KeysAsync(pattern: $"{redisPrefix}*").WithCancellation(cancellationToken))
                {
                    if (await database.KeyDeleteAsync(key))
                    {
                        totalRemoved++;
                    }
                }
            }

            _logger.LogInformation("CacheRemove {CachePrefix} {RemovedCount}", prefix, totalRemoved);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RedisFailure {Operation} {CachePrefix}", nameof(RemoveByPrefixAsync), prefix);
        }
    }

    public async Task<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T?>> factory,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default) where T : class
    {
        var cached = await GetAsync<T>(key, cancellationToken);
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
}
