using System.Text.Json;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.GridPreferences;
using Fgs.User.Application.Features.GridPreferences.Commands.DeleteFgsUserGridPreference;
using Fgs.User.Application.Features.GridPreferences.Commands.SaveFgsUserGridPreference;
using Fgs.User.Application.Features.GridPreferences.Dtos;
using Fgs.User.Application.Features.GridPreferences.Queries.GetFgsUserGridPreference;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class FgsUserGridPreferenceHandlerTests
{
    private static readonly Guid UserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private const long TenantId = 10;

    private const long CompanyId = 20;

    private const string GridCode = "WorkOrderList";

    private const string SavedConfiguration =
        """{"columns":[{"field":"name","width":180,"visible":true}],"sorting":[{"field":"name","direction":"asc"}]}""";

    [Fact]
    public async Task Get_CachesEmptyResult_PutOverwrites_DeleteRemoves_NextGetLoadsAgain()
    {
        var entries = new Dictionary<string, FgsUserGridPreferenceDto>(StringComparer.Ordinal);
        var cache = CreateCache(entries);
        var store = new Mock<IFgsUserGridPreferenceStore>();
        store
            .Setup(s => s.GetAsync(GridCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsUserGridPreferenceDto(GridCode, EmptyConfiguration()));
        store
            .Setup(s => s.UpsertAsync(GridCode, SavedConfiguration, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsUserGridPreferenceDto(GridCode, ParseConfiguration(SavedConfiguration)));
        store
            .Setup(s => s.DeleteAsync(GridCode, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenant = new TenantContext { TenantId = TenantId, CompanyId = CompanyId };
        var user = SignedInUser();
        var getHandler = new GetFgsUserGridPreferenceQueryHandler(store.Object, cache.Object, tenant, user.Object);
        var saveHandler = new SaveFgsUserGridPreferenceCommandHandler(store.Object, cache.Object, tenant, user.Object);
        var deleteHandler = new DeleteFgsUserGridPreferenceCommandHandler(store.Object, cache.Object, tenant, user.Object);
        var expectedKey = CacheKeys.Build(TenantId, CompanyId, "user-grid-preference", $"{UserId:D}:{GridCode}");

        var first = await getHandler.Handle(new GetFgsUserGridPreferenceQuery(GridCode), CancellationToken.None);
        var second = await getHandler.Handle(new GetFgsUserGridPreferenceQuery(GridCode), CancellationToken.None);

        first.Success.Should().BeTrue();
        first.Data!.GridConfiguration.GetRawText().Should().Be("{}");
        second.Data!.GridConfiguration.GetRawText().Should().Be("{}");
        entries.Keys.Should().ContainSingle().Which.Should().Be(expectedKey);
        store.Verify(s => s.GetAsync(GridCode, It.IsAny<CancellationToken>()), Times.Once);

        var saved = await saveHandler.Handle(
            new SaveFgsUserGridPreferenceCommand(GridCode, ParseConfiguration(SavedConfiguration)),
            CancellationToken.None);

        saved.Success.Should().BeTrue();
        saved.Data!.GridConfiguration.GetRawText().Should().Be(SavedConfiguration);
        entries[expectedKey].GridConfiguration.GetRawText().Should().Be(SavedConfiguration);

        var deleted = await deleteHandler.Handle(
            new DeleteFgsUserGridPreferenceCommand(GridCode),
            CancellationToken.None);

        deleted.Success.Should().BeTrue();
        deleted.StatusCode.Should().Be(ApiStatusCodes.Ok);
        entries.Should().NotContainKey(expectedKey);
        store.Verify(s => s.DeleteAsync(GridCode, It.IsAny<CancellationToken>()), Times.Once);

        var afterDelete = await getHandler.Handle(new GetFgsUserGridPreferenceQuery(GridCode), CancellationToken.None);

        afterDelete.Data!.GridConfiguration.GetRawText().Should().Be("{}");
        entries[expectedKey].GridConfiguration.GetRawText().Should().Be("{}");
        store.Verify(s => s.GetAsync(GridCode, It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Save_EmptyConfiguration_OverwritesCacheWithEmptyObject()
    {
        var entries = new Dictionary<string, FgsUserGridPreferenceDto>(StringComparer.Ordinal);
        var cache = CreateCache(entries);
        var store = new Mock<IFgsUserGridPreferenceStore>();
        store
            .Setup(s => s.UpsertAsync(GridCode, "{}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsUserGridPreferenceDto(GridCode, EmptyConfiguration()));

        var handler = new SaveFgsUserGridPreferenceCommandHandler(
            store.Object,
            cache.Object,
            new TenantContext { TenantId = TenantId, CompanyId = CompanyId },
            SignedInUser().Object);

        var response = await handler.Handle(
            new SaveFgsUserGridPreferenceCommand(GridCode, EmptyConfiguration()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.GridConfiguration.GetRawText().Should().Be("{}");
        var expectedKey = CacheKeys.Build(TenantId, CompanyId, "user-grid-preference", $"{UserId:D}:{GridCode}");
        entries[expectedKey].GridConfiguration.GetRawText().Should().Be("{}");
    }

    [Fact]
    public async Task Save_StringifiedConfiguration_PassesParsedJsonToStore()
    {
        var store = new Mock<IFgsUserGridPreferenceStore>();
        store
            .Setup(s => s.UpsertAsync(GridCode, SavedConfiguration, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsUserGridPreferenceDto(GridCode, ParseConfiguration(SavedConfiguration)));
        var handler = new SaveFgsUserGridPreferenceCommandHandler(
            store.Object,
            Mock.Of<ICacheService>(),
            new TenantContext { TenantId = TenantId, CompanyId = CompanyId },
            SignedInUser().Object);

        var response = await handler.Handle(
            new SaveFgsUserGridPreferenceCommand(GridCode, JsonSerializer.SerializeToElement(SavedConfiguration)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.GridConfiguration.GetRawText().Should().Be(SavedConfiguration);
        store.Verify(s => s.UpsertAsync(GridCode, SavedConfiguration, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Get_WhenCallerIsNotSignedIn_ReturnsUnauthorizedWithoutCacheOrStore()
    {
        var store = new Mock<IFgsUserGridPreferenceStore>();
        var cache = new Mock<ICacheService>();
        var user = new Mock<IFgsUserContext>();
        user.SetupGet(c => c.IsAuthenticated).Returns(false);
        user.SetupGet(c => c.UserId).Returns(UserId);

        var handler = new GetFgsUserGridPreferenceQueryHandler(
            store.Object,
            cache.Object,
            new TenantContext { TenantId = TenantId, CompanyId = CompanyId },
            user.Object);

        var response = await handler.Handle(new GetFgsUserGridPreferenceQuery(GridCode), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.Unauthorized);
        store.Verify(s => s.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        cache.Verify(
            c => c.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<FgsUserGridPreferenceDto>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Get_WhenUserIdMissing_ReturnsUnauthorized()
    {
        var user = new Mock<IFgsUserContext>();
        user.SetupGet(c => c.IsAuthenticated).Returns(true);
        user.SetupGet(c => c.UserId).Returns((Guid?)null);
        var handler = new GetFgsUserGridPreferenceQueryHandler(
            Mock.Of<IFgsUserGridPreferenceStore>(),
            Mock.Of<ICacheService>(),
            new TenantContext { TenantId = TenantId, CompanyId = CompanyId },
            user.Object);

        var response = await handler.Handle(new GetFgsUserGridPreferenceQuery(GridCode), CancellationToken.None);

        response.StatusCode.Should().Be(ApiStatusCodes.Unauthorized);
    }

    [Fact]
    public async Task Save_WhenCallerIsNotSignedIn_ReturnsUnauthorized()
    {
        var store = new Mock<IFgsUserGridPreferenceStore>();
        var user = new Mock<IFgsUserContext>();
        user.SetupGet(c => c.IsAuthenticated).Returns(false);
        var handler = new SaveFgsUserGridPreferenceCommandHandler(
            store.Object,
            Mock.Of<ICacheService>(),
            new TenantContext { TenantId = TenantId, CompanyId = CompanyId },
            user.Object);

        var response = await handler.Handle(
            new SaveFgsUserGridPreferenceCommand(GridCode, ParseConfiguration(SavedConfiguration)),
            CancellationToken.None);

        response.StatusCode.Should().Be(ApiStatusCodes.Unauthorized);
        store.Verify(
            s => s.UpsertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Delete_WhenCallerIsNotSignedIn_ReturnsUnauthorized()
    {
        var store = new Mock<IFgsUserGridPreferenceStore>();
        var cache = new Mock<ICacheService>();
        var user = new Mock<IFgsUserContext>();
        user.SetupGet(c => c.IsAuthenticated).Returns(true);
        user.SetupGet(c => c.UserId).Returns((Guid?)null);
        var handler = new DeleteFgsUserGridPreferenceCommandHandler(
            store.Object,
            cache.Object,
            new TenantContext { TenantId = TenantId, CompanyId = CompanyId },
            user.Object);

        var response = await handler.Handle(new DeleteFgsUserGridPreferenceCommand(GridCode), CancellationToken.None);

        response.StatusCode.Should().Be(ApiStatusCodes.Unauthorized);
        store.Verify(s => s.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        cache.Verify(c => c.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private static JsonElement EmptyConfiguration() => ParseConfiguration("{}");

    private static JsonElement ParseConfiguration(string json) => JsonDocument.Parse(json).RootElement.Clone();

    private static Mock<IFgsUserContext> SignedInUser()
    {
        var user = new Mock<IFgsUserContext>();
        user.SetupGet(c => c.IsAuthenticated).Returns(true);
        user.SetupGet(c => c.UserId).Returns(UserId);
        user.SetupGet(c => c.DisplayName).Returns("Ada Lovelace");
        return user;
    }

    private static Mock<ICacheService> CreateCache(Dictionary<string, FgsUserGridPreferenceDto> entries)
    {
        var cache = new Mock<ICacheService>();
        cache
            .Setup(c => c.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<FgsUserGridPreferenceDto>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()))
            .Returns<string, Func<Task<FgsUserGridPreferenceDto>>, TimeSpan?, CancellationToken>(
                async (key, factory, expiration, _) =>
                {
                    expiration.Should().BeNull();
                    if (entries.TryGetValue(key, out var existing))
                    {
                        return existing;
                    }

                    var created = await factory();
                    entries[key] = created;
                    return created;
                });

        cache
            .Setup(c => c.SetAsync(
                It.IsAny<string>(),
                It.IsAny<FgsUserGridPreferenceDto>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, FgsUserGridPreferenceDto, TimeSpan?, CancellationToken>((key, value, expiration, _) =>
            {
                expiration.Should().BeNull();
                entries[key] = value;
            })
            .Returns(Task.CompletedTask);

        cache
            .Setup(c => c.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((key, _) => entries.Remove(key))
            .Returns(Task.CompletedTask);

        return cache;
    }
}
