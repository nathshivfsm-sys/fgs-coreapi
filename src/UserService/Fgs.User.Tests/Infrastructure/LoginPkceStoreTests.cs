using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Infrastructure.Common.Identity;
using Moq;

namespace Fgs.User.Tests.Infrastructure;

public sealed class LoginPkceStoreTests
{
    [Fact]
    public async Task TakeAsync_ReturnsValueFromAtomicRemove()
    {
        var userId = Guid.NewGuid();
        var state = new LoginPkceState("verifier", "https://app.local/callback", userId);
        var cache = new Mock<ICacheService>();
        cache
            .Setup(c => c.GetAndRemoveAsync<LoginPkceState>(
                CacheKeys.LoginPkceByState("abc"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(state);

        var store = new LoginPkceStore(cache.Object);

        var result = await store.TakeAsync("abc");

        result.Should().Be(state);
        cache.Verify(
            c => c.GetAsync<LoginPkceState>(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
        cache.Verify(
            c => c.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task TakeAsync_SecondTake_ReturnsNull()
    {
        var cache = new Mock<ICacheService>();
        cache
            .SetupSequence(c => c.GetAndRemoveAsync<LoginPkceState>(
                CacheKeys.LoginPkceByState("state"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LoginPkceState("verifier", "https://app.local/callback", Guid.NewGuid()))
            .ReturnsAsync((LoginPkceState?)null);

        var store = new LoginPkceStore(cache.Object);

        (await store.TakeAsync("state")).Should().NotBeNull();
        (await store.TakeAsync("state")).Should().BeNull();
    }
}
