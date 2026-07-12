using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Application.Features.Auth.Commands.EntraLoginCallback;
using Fgs.Persistence.Abstractions;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Infrastructure.Database;
using Fgs.Persistence.Implementations;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class EntraLoginCallbackCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidUserState_ReturnsAccessToken()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var userId = Guid.NewGuid();
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = 1,
            CompanyId = 1,
            Email = "user@test.com",
            DisplayName = "User",
            EntraObjectId = "oid-123",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var profileStore = new Mock<IUserAuthProfileStore>();
        var handler = CreateHandler(context, "user@test.com", profileStore.Object);
        var result = await handler.Handle(
            new EntraLoginCallbackCommand("code", $"{OAuthStatePrefixes.UserLogin}{userId}"),
            CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Data!.AccessToken.Should().Be("entra-access-token");
        profileStore.Verify(s => s.SetAsync(It.IsAny<Fgs.Contracts.Auth.UserAuthProfileDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInactiveUser_ReturnsForbidden()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var userId = Guid.NewGuid();
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = 1,
            CompanyId = 1,
            Email = "user@test.com",
            DisplayName = "User",
            IsActive = false,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var result = await CreateHandler(context, "user@test.com").Handle(
            new EntraLoginCallbackCommand("code", $"{OAuthStatePrefixes.UserLogin}{userId}"),
            CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(AuthErrorMessages.UserNotActive);
    }

    [Fact]
    public async Task Handle_WithEmailMismatch_ReturnsBadRequest()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var userId = Guid.NewGuid();
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = 1,
            CompanyId = 1,
            Email = "user@test.com",
            DisplayName = "User",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var result = await CreateHandler(context, "other@test.com").Handle(
            new EntraLoginCallbackCommand("code", $"{OAuthStatePrefixes.UserLogin}{userId}"),
            CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(AuthErrorMessages.EntraEmailMismatch);
    }

    private static EntraLoginCallbackCommandHandler CreateHandler(
        FgsUserDbContext context,
        string entraEmail,
        IUserAuthProfileStore? profileStore = null)
    {
        var entraMock = new Mock<IEntraExternalIdService>();
        entraMock
            .Setup(s => s.ExchangeCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EntraTokenResult("entra-access-token", "oid-123", entraEmail, "User"));

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConfigurationKeys.EntraExternalId.RedirectUri] = "https://localhost/callback",
                [ConfigurationKeys.Application.DashboardUrl] = "https://localhost/dashboard"
            })
            .Build();

        return new EntraLoginCallbackCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            entraMock.Object,
            new EmailNormalizer(),
            new DateTimeProvider(),
            configuration,
            profileStore ?? Mock.Of<IUserAuthProfileStore>(),
            TestUserRepositories.RoleCodesRead(context));
    }
}
