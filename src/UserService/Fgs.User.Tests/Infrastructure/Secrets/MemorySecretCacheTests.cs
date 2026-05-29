using Fgs.User.Infrastructure.Common.Options;
using Fgs.User.Infrastructure.Secrets;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Fgs.User.Tests.Infrastructure.Secrets;

public sealed class MemorySecretCacheTests
{
    [Fact]
    public void TryGet_returns_cached_secret_json()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        var sut = new MemorySecretCache(
            cache,
            Options.Create(new AwsCredentialsOptions { CacheTtlSeconds = 60 }));

        var key = MemorySecretCache.BuildCacheKey(1, 2, Guid.NewGuid(), 1);
        sut.Set(key, """{"server":"host"}""");

        sut.TryGet(key, out var json).Should().BeTrue();
        json.Should().Be("""{"server":"host"}""");
    }

    [Fact]
    public void Invalidate_removes_cached_entry()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        var sut = new MemorySecretCache(
            cache,
            Options.Create(new AwsCredentialsOptions { CacheTtlSeconds = 60 }));

        var secretId = Guid.NewGuid();
        var key = MemorySecretCache.BuildCacheKey(1, 2, secretId, 1);
        sut.Set(key, "value");
        sut.Invalidate(1, 2, secretId);

        sut.TryGet(key, out _).Should().BeFalse();
    }
}
