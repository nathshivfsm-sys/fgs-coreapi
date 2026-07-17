using Fgs.Contracts.Auth;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Security.UserAuth;
using Moq;

namespace Fgs.Security.Tests.UserAuth;

public sealed class UserAuthProfileStoreTests
{
    [Fact]
    public async Task GetOrLoadAsync_OnCacheMiss_LoadsFromSourceAndCaches()
    {
        var cache = new Mock<ICacheService>();
        var source = new Mock<IUserAuthProfileSource>();
        var profile = CreateProfile();

        cache
            .Setup(c => c.GetAsync<UserAuthProfileDto>(
                CacheKeys.UserAuthByEntraObjectId("oid-1"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserAuthProfileDto?)null);

        source
            .Setup(s => s.LoadByEntraObjectIdAsync("oid-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var store = new UserAuthProfileStore(
            cache.Object,
            source.Object,
            Microsoft.Extensions.Options.Options.Create(new UserAuthCacheOptions()));

        var result = await store.GetOrLoadAsync("oid-1", CancellationToken.None);

        result.Should().BeEquivalentTo(profile);
        source.Verify(s => s.LoadByEntraObjectIdAsync("oid-1", It.IsAny<CancellationToken>()), Times.Once);
        cache.Verify(
            c => c.SetAsync(
                CacheKeys.UserAuthByEntraObjectId("oid-1"),
                profile,
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task InvalidateAsync_RemovesOidAndUserKeys()
    {
        var cache = new Mock<ICacheService>();
        var store = new UserAuthProfileStore(
            cache.Object,
            Mock.Of<IUserAuthProfileSource>(),
            Microsoft.Extensions.Options.Options.Create(new UserAuthCacheOptions()));

        var userId = Guid.NewGuid();
        await store.InvalidateAsync(userId, "oid-1", CancellationToken.None);

        cache.Verify(c => c.RemoveAsync(CacheKeys.UserAuthByUserId(userId), It.IsAny<CancellationToken>()), Times.Once);
        cache.Verify(c => c.RemoveAsync(CacheKeys.UserAuthByEntraObjectId("oid-1"), It.IsAny<CancellationToken>()), Times.Once);
    }

    private static UserAuthProfileDto CreateProfile() =>
        new(
            Guid.NewGuid(),
            "user@test.com",
            "oid-1",
            1,
            1,
            true,
            false,
            ["TENANT_ADMIN"],
            [],
            [],
            []);
}
