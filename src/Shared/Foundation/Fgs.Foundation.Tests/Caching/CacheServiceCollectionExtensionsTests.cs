using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Caching.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Foundation.Tests.Caching;

public sealed class CacheServiceCollectionExtensionsTests
{
    [Fact]
    public void AddFgsRedisCache_WhenDisabled_RegistersNullCacheService()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Redis:Enabled"] = "false"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddFgsRedisCache(configuration);

        using var provider = services.BuildServiceProvider();
        var cache = provider.GetRequiredService<ICacheService>();

        cache.Should().BeOfType<Fgs.Foundation.Caching.NullCacheService>();
    }

    [Fact]
    public void AddFgsRedisCache_WhenConnectionStringMissing_RegistersNullCacheService()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Redis:Enabled"] = "true",
                ["Redis:ConnectionString"] = ""
            })
            .Build();

        var services = new ServiceCollection();
        services.AddFgsRedisCache(configuration);

        using var provider = services.BuildServiceProvider();
        var cache = provider.GetRequiredService<ICacheService>();

        cache.Should().BeOfType<Fgs.Foundation.Caching.NullCacheService>();
    }

    [Fact]
    public void AddFgsRedisCache_WhenEnabled_RegistersRedisCacheService()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Redis:Enabled"] = "true",
                ["Redis:ConnectionString"] = "localhost:6379"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddFgsRedisCache(configuration);

        services.Should().Contain(descriptor =>
            descriptor.ServiceType == typeof(ICacheService)
            && descriptor.ImplementationFactory != null);
    }
}
