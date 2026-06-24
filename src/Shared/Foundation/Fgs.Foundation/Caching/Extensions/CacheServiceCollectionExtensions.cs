using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Caching.HealthChecks;
using Fgs.Foundation.Caching.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Fgs.Foundation.Caching.Extensions;

public static class CacheServiceCollectionExtensions
{
    public static IServiceCollection AddFgsRedisCache(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RedisCacheOptions>(configuration.GetSection(RedisCacheOptions.SectionName));

        services.AddOptions<Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions>()
            .Configure<IOptions<RedisCacheOptions>>((redisCacheOptions, fgsRedisOptions) =>
            {
                var options = fgsRedisOptions.Value;
                redisCacheOptions.Configuration = NormalizeConnectionString(options.ConnectionString);
                redisCacheOptions.InstanceName = options.InstanceName;
            });

        services.AddStackExchangeRedisCache(_ => { });

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RedisCacheOptions>>().Value;
            if (!IsRedisConfigured(options))
            {
                throw new InvalidOperationException("Redis is disabled or ConnectionString is missing.");
            }

            return ConnectionMultiplexer.Connect(NormalizeConnectionString(options.ConnectionString));
        });

        services.AddSingleton<ICacheService>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RedisCacheOptions>>().Value;
            if (!IsRedisConfigured(options))
            {
                return new NullCacheService();
            }

            return new RedisCacheService(
                sp.GetRequiredService<Microsoft.Extensions.Caching.Distributed.IDistributedCache>(),
                sp.GetRequiredService<IConnectionMultiplexer>(),
                sp.GetRequiredService<IOptions<RedisCacheOptions>>(),
                sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RedisCacheService>>());
        });

        services.AddSingleton<RedisHealthCheck>();
        services.AddHealthChecks()
            .AddCheck<RedisHealthCheck>("redis", failureStatus: HealthStatus.Unhealthy, tags: ["ready"]);

        return services;
    }

    internal static bool IsRedisConfigured(RedisCacheOptions options) =>
        options.Enabled && !string.IsNullOrWhiteSpace(options.ConnectionString);

    internal static string NormalizeConnectionString(string connectionString)
    {
        if (connectionString.Contains("abortConnect=", StringComparison.OrdinalIgnoreCase))
        {
            return connectionString;
        }

        return $"{connectionString.TrimEnd(',')},abortConnect=false";
    }
}
