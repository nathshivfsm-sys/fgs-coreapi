using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Features.Auth.Queries.GetUserAuthProfile;
using Fgs.User.Infrastructure.Common.Identity;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class GetUserAuthProfileQueryHandlerTests
{
    [Fact]
    public async Task Handle_WithValidProfile_CachesAndReturnsDto()
    {
        var profile = new FgsUserProfile(
            Guid.NewGuid(),
            "user@test.com",
            "oid-123",
            1,
            1,
            true,
            false,
            ["TENANT_ADMIN"],
            [],
            [],
            []);

        var resolver = new Mock<IFgsUserProfileResolver>();
        resolver
            .Setup(r => r.ResolveByEntraObjectIdAsync("oid-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var profileStore = new Mock<IUserAuthProfileStore>();
        var handler = new GetUserAuthProfileQueryHandler(resolver.Object, profileStore.Object);

        var result = await handler.Handle(new GetUserAuthProfileQuery("oid-123"), CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Data!.Email.Should().Be("user@test.com");
        profileStore.Verify(
            s => s.SetAsync(
                It.Is<Fgs.Contracts.Auth.UserAuthProfileDto>(d => d.EntraObjectId == "oid-123"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProfileMissing_ReturnsNotFound()
    {
        var resolver = new Mock<IFgsUserProfileResolver>();
        resolver
            .Setup(r => r.ResolveByEntraObjectIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsUserProfile?)null);

        var handler = new GetUserAuthProfileQueryHandler(
            resolver.Object,
            Mock.Of<IUserAuthProfileStore>());

        var result = await handler.Handle(new GetUserAuthProfileQuery("missing"), CancellationToken.None);

        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(Fgs.Contracts.Api.ApiStatusCodes.NotFound);
    }
}
