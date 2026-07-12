using Fgs.User.Application.Abstractions.Identity;
using Fgs.Contracts.Api;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth;
using Fgs.Persistence.Abstractions;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Application.Features.Auth.Commands.StartLogin;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Infrastructure.Database;
using Fgs.Persistence.Implementations;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class StartLoginCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithActiveUser_ReturnsEntraRedirectUrl()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var userId = Guid.NewGuid();
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = 1,
            CompanyId = 1,
            Email = "ACTIVE@TEST.COM",
            DisplayName = "Active User",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var entraMock = new Mock<IEntraExternalIdService>();
        entraMock
            .Setup(s => s.BuildAuthorizationUrl(
                $"{OAuthStatePrefixes.UserLogin}{userId}",
                It.IsAny<string>(),
                "ACTIVE@TEST.COM"))
            .Returns("https://login.example/authorize");

        var handler = CreateHandler(context, entraMock.Object);
        var result = await handler.Handle(new StartLoginCommand("active@test.com"), CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Data!.RedirectUrl.Should().Be("https://login.example/authorize");
    }

    [Fact]
    public async Task Handle_WithInactiveUser_ReturnsLoginNotAvailable()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        context.FgsUsers.Add(new FgsUser
        {
            Id = Guid.NewGuid(),
            TenantId = 1,
            CompanyId = 1,
            Email = "inactive@test.com",
            DisplayName = "Inactive",
            IsActive = false,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var result = await CreateHandler(context).Handle(
            new StartLoginCommand("inactive@test.com"),
            CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(AuthErrorMessages.LoginNotAvailable);
    }

    [Fact]
    public async Task Handle_WithUnknownEmail_ReturnsLoginNotAvailable()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var result = await CreateHandler(context).Handle(
            new StartLoginCommand("missing@test.com"),
            CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(AuthErrorMessages.LoginNotAvailable);
    }

    private static StartLoginCommandHandler CreateHandler(
        FgsUserDbContext context,
        IEntraExternalIdService? entraService = null)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConfigurationKeys.EntraExternalId.RedirectUri] = "https://localhost/callback"
            })
            .Build();

        return new StartLoginCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            entraService ?? Mock.Of<IEntraExternalIdService>(),
            new EmailNormalizer(),
            configuration);
    }
}
