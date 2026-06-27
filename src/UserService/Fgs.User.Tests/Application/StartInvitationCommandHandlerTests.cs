using Fgs.MultiTenancy;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.Contracts.Api;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Invitations;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Features.Invitations.Commands.StartInvitation;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.Security.Constants;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Infrastructure.Database;
using Fgs.Persistence.Implementations;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class StartInvitationCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidToken_ReturnsEntraRedirect()
    {
        var tokenService = new InvitationTokenService();
        var plain = tokenService.GenerateToken();
        var hash = tokenService.HashToken(plain);

        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = 1,
            CompanyId = 1,
            Email = "a@test.com",
            DisplayName = "Acme Admin",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsInvitations.Add(new FgsInvitation
        {
            Id = invitationId,
            UserId = userId,
            TenantId = 1,
            Email = "a@test.com",
            TokenHash = hash,
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(1),
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var entraMock = new Mock<IEntraExternalIdService>();
        entraMock
            .Setup(s => s.BuildAuthorizationUrl(
                invitationId,
                It.IsAny<string>(),
                "a@test.com"))
            .Returns("https://login.example/authorize");

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConfigurationKeys.EntraExternalId.RedirectUri] = "https://localhost/callback",
                ["EntraExternalId:UserFlow"] = "SignUpSignIn"
            })
            .Build();

        var handler = new StartInvitationCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            tokenService,
            entraMock.Object,
            new DateTimeProvider(),
            configuration);

        var result = await handler.Handle(new StartInvitationCommand(plain), CancellationToken.None);

        result.Success.Should().BeTrue();
        result.RedirectUrl.Should().StartWith("https://login.example");
    }

    [Fact]
    public async Task Handle_WithAcceptedInvitation_ReturnsEntraLoginRedirect()
    {
        var tokenService = new InvitationTokenService();
        var plain = tokenService.GenerateToken();
        var hash = tokenService.HashToken(plain);

        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = Guid.NewGuid();
        context.FgsInvitations.Add(new FgsInvitation
        {
            Id = invitationId,
            UserId = Guid.NewGuid(),
            TenantId = 1,
            Email = "verified@test.com",
            TokenHash = hash,
            Status = InvitationStatus.Accepted,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(-1),
            AcceptedAtUtc = DateTimeOffset.UtcNow.AddDays(-1),
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var entraMock = new Mock<IEntraExternalIdService>();
        entraMock
            .Setup(s => s.BuildAuthorizationUrl(
                invitationId,
                It.IsAny<string>(),
                "verified@test.com"))
            .Returns("https://login.example/signin");

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConfigurationKeys.EntraExternalId.RedirectUri] = "https://localhost/callback"
            })
            .Build();

        var handler = new StartInvitationCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            tokenService,
            entraMock.Object,
            new DateTimeProvider(),
            configuration);

        var result = await handler.Handle(new StartInvitationCommand(plain), CancellationToken.None);

        result.Success.Should().BeTrue();
        result.RedirectUrl.Should().StartWith("https://login.example/signin");
    }

    [Fact]
    public async Task Handle_WhenTokenMissing_ReturnsError()
    {
        var handler = CreateHandler(await TestDbContextFactory.CreateAndInitializeAsync());
        var result = await handler.Handle(new StartInvitationCommand(string.Empty), CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be(InvitationErrorMessages.TokenRequired);
    }

    [Fact]
    public async Task Handle_WhenTokenInvalid_ReturnsError()
    {
        var handler = CreateHandler(await TestDbContextFactory.CreateAndInitializeAsync());
        var result = await handler.Handle(new StartInvitationCommand("bad-token"), CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be(InvitationErrorMessages.InvalidToken);
    }

    [Fact]
    public async Task Handle_WhenInvitationExpired_ReturnsError()
    {
        var tokenService = new InvitationTokenService();
        var plain = tokenService.GenerateToken();
        var hash = tokenService.HashToken(plain);

        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        context.FgsInvitations.Add(new FgsInvitation
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            TenantId = 1,
            Email = "expired@test.com",
            TokenHash = hash,
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(-1),
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var result = await CreateHandler(context).Handle(new StartInvitationCommand(plain), CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be(InvitationErrorMessages.Expired);
    }

    [Fact]
    public async Task Handle_WhenTenantContextDoesNotMatchInvitation_StillFindsToken()
    {
        var tokenService = new InvitationTokenService();
        var plain = tokenService.GenerateToken();
        var hash = tokenService.HashToken(plain);

        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = 99, CompanyId = 1 }
        };
        var context = await TestDbContextFactory.CreateAndInitializeAsync(accessor);
        var invitationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = 1,
            CompanyId = 1,
            Email = "a@test.com",
            DisplayName = "Acme Admin",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsInvitations.Add(new FgsInvitation
        {
            Id = invitationId,
            UserId = userId,
            TenantId = 1,
            Email = "a@test.com",
            TokenHash = hash,
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(1),
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var entraMock = new Mock<IEntraExternalIdService>();
        entraMock
            .Setup(s => s.BuildAuthorizationUrl(
                invitationId,
                It.IsAny<string>(),
                "a@test.com"))
            .Returns("https://login.example/authorize");

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConfigurationKeys.EntraExternalId.RedirectUri] = "https://localhost/callback"
            })
            .Build();

        var handler = new StartInvitationCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            tokenService,
            entraMock.Object,
            new DateTimeProvider(),
            configuration);

        var result = await handler.Handle(new StartInvitationCommand(plain), CancellationToken.None);

        result.Success.Should().BeTrue();
        result.RedirectUrl.Should().Be("https://login.example/authorize");
    }

    [Fact]
    public async Task Handle_WhenInvitationRevoked_ReturnsNotActive()
    {
        var tokenService = new InvitationTokenService();
        var plain = tokenService.GenerateToken();
        var hash = tokenService.HashToken(plain);

        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        context.FgsInvitations.Add(new FgsInvitation
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            TenantId = 1,
            Email = "revoked@test.com",
            TokenHash = hash,
            Status = InvitationStatus.Expired,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(1),
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var result = await CreateHandler(context).Handle(new StartInvitationCommand(plain), CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be(InvitationErrorMessages.NotActive);
    }

    private static StartInvitationCommandHandler CreateHandler(FgsUserDbContext context)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConfigurationKeys.EntraExternalId.RedirectUri] = "https://localhost/callback"
            })
            .Build();

        return new StartInvitationCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            new InvitationTokenService(),
            Mock.Of<IEntraExternalIdService>(),
            new DateTimeProvider(),
            configuration);
    }
}

