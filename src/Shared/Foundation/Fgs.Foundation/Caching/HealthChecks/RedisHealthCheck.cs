using Fgs.Foundation.Caching.Extensions;
using Fgs.Foundation.Caching.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Fgs.Foundation.Caching.HealthChecks;

internal sealed class RedisHealthCheck(
    IOptions<RedisCacheOptions> options,
    IServiceProvider serviceProvider) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var redisOptions = options.Value;
        if (!CacheServiceCollectionExtensions.IsRedisConfigured(redisOptions))
        {
            return HealthCheckResult.Healthy("Redis caching is disabled.");
        }

        try
        {
            var multiplexer = serviceProvider.GetRequiredService<IConnectionMultiplexer>();
            await multiplexer.GetDatabase().PingAsync();
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Redis is unavailable.", ex);
        }
    }
}
