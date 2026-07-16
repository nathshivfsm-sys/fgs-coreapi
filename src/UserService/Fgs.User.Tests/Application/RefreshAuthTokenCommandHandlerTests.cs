using Fgs.Contracts.Api;
using Fgs.Persistence.Implementations;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Application.Features.Auth.Commands.RefreshAuthToken;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class RefreshAuthTokenCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidRefreshToken_ReturnsLoginProfile()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        context.FgsUsers.Add(new FgsUser
        {
            Id = Guid.NewGuid(),
            TenantId = 1,
            CompanyId = 1,
            Email = "user@test.com",
            DisplayName = "User Name",
            EntraObjectId = "oid-123",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var entra = new Mock<IEntraExternalIdService>();
        entra
            .Setup(s => s.RefreshTokenAsync("refresh", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EntraTokenResult(
                "new-access",
                "oid-123",
                "user@test.com",
                "User Name",
                "refresh2",
                "id",
                3600,
                "Bearer"));

        var profileStore = new Mock<IUserAuthProfileStore>();
        var handler = new RefreshAuthTokenCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            entra.Object,
            TestUserRepositories.ProfileBuilder(),
            profileStore.Object);

        var result = await handler.Handle(new RefreshAuthTokenCommand("refresh"), CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Data!.AccessToken.Should().Be("new-access");
        result.Data.User.Email.Should().Be("user@test.com");
    }

    [Fact]
    public async Task Handle_WhenRefreshFails_ReturnsUnauthorized()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var entra = new Mock<IEntraExternalIdService>();
        entra
            .Setup(s => s.RefreshTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("denied"));

        var handler = new RefreshAuthTokenCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            entra.Object,
            TestUserRepositories.ProfileBuilder(),
            Mock.Of<IUserAuthProfileStore>());

        var result = await handler.Handle(new RefreshAuthTokenCommand("bad"), CancellationToken.None);

        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(ApiStatusCodes.Unauthorized);
        result.Errors.Should().Contain(AuthErrorMessages.RefreshTokenFailed);
    }
}
