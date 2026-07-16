using Fgs.Contracts.Api;
using Fgs.Persistence.Implementations;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Application.Features.Auth.Commands.ExchangeLoginCode;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Database;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class ExchangeLoginCodeCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCodeAndState_ReturnsLoginProfile()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var userId = Guid.NewGuid();
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = 1,
            CompanyId = 1,
            Email = "user@test.com",
            DisplayName = "Ada Lovelace",
            EntraObjectId = "oid-123",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var state = $"{OAuthStatePrefixes.UserLogin}{userId}";
        var pkceStore = new Mock<ILoginPkceStore>();
        pkceStore
            .Setup(s => s.TakeAsync(state, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LoginPkceState("verifier", "https://localhost:3000/auth/callback", userId));

        var entra = new Mock<IEntraExternalIdService>();
        entra
            .Setup(s => s.ExchangeLoginCodeAsync(
                "code",
                "https://localhost:3000/auth/callback",
                "verifier",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EntraTokenResult(
                "access",
                "oid-123",
                "user@test.com",
                "Ada Lovelace",
                "refresh",
                "id",
                3600,
                "Bearer"));

        var profileStore = new Mock<IUserAuthProfileStore>();
        var handler = new ExchangeLoginCodeCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            entra.Object,
            new EmailNormalizer(),
            pkceStore.Object,
            TestUserRepositories.ProfileBuilder(),
            profileStore.Object);

        var result = await handler.Handle(new ExchangeLoginCodeCommand("code", state), CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Data!.AccessToken.Should().Be("access");
        result.Data.RefreshToken.Should().Be("refresh");
        result.Data.User.Email.Should().Be("user@test.com");
        result.Data.User.FirstName.Should().Be("Ada");
        profileStore.Verify(s => s.SetAsync(It.IsAny<Fgs.Contracts.Auth.UserAuthProfileDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithExpiredPkce_ReturnsBadRequest()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var userId = Guid.NewGuid();
        var state = $"{OAuthStatePrefixes.UserLogin}{userId}";
        var pkceStore = new Mock<ILoginPkceStore>();
        pkceStore
            .Setup(s => s.TakeAsync(state, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginPkceState?)null);

        var handler = new ExchangeLoginCodeCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            Mock.Of<IEntraExternalIdService>(),
            new EmailNormalizer(),
            pkceStore.Object,
            TestUserRepositories.ProfileBuilder(),
            Mock.Of<IUserAuthProfileStore>());

        var result = await handler.Handle(new ExchangeLoginCodeCommand("code", state), CancellationToken.None);

        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        result.Errors.Should().Contain(AuthErrorMessages.PkceStateExpired);
    }
}
