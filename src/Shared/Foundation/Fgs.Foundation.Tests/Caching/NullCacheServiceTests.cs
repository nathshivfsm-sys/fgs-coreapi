using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Caching.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;

namespace Fgs.Foundation.Tests.Caching;

public sealed class NullCacheServiceTests
{
    private readonly NullCacheService _sut = new();

    [Fact]
    public async Task GetAsync_AlwaysReturnsNull()
    {
        var result = await _sut.GetAsync<string>("any-key");

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetOrSetAsync_AlwaysInvokesFactory()
    {
        var invoked = false;
        var result = await _sut.GetOrSetAsync("key", async () =>
        {
            invoked = true;
            await Task.CompletedTask;
            return "value";
        });

        invoked.Should().BeTrue();
        result.Should().Be("value");
    }

    [Fact]
    public async Task SetAndRemove_DoNotThrow()
    {
        await _sut.SetAsync("key", "value");
        await _sut.RemoveAsync("key");
        await _sut.RemoveByPrefixAsync("prefix:");
    }
}
