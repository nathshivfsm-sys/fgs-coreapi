using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Features.Invitations.Queries.StartInvitation;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Persistence;
using Fgs.User.Infrastructure.Security;
using Fgs.User.Infrastructure.Time;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class StartInvitationQueryHandlerTests
{
    [Fact]
    public async Task Handle_WithValidToken_ReturnsEntraRedirect()
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
            TenantId = Guid.NewGuid(),
            Email = "a@test.com",
            TokenHash = hash,
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(1),
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var entraMock = new Mock<IEntraExternalIdService>();
        entraMock
            .Setup(s => s.BuildAuthorizationUrl(invitationId, It.IsAny<string>(), "a@test.com"))
            .Returns("https://login.example/authorize");

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["EntraExternalId:RedirectUri"] = "https://localhost/callback",
                ["EntraExternalId:UserFlow"] = "SignUpSignIn"
            })
            .Build();

        var handler = new StartInvitationQueryHandler(
            new UnitOfWork(context),
            tokenService,
            entraMock.Object,
            new DateTimeProvider(),
            configuration);

        var result = await handler.Handle(new StartInvitationQuery(plain), CancellationToken.None);

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
            TenantId = Guid.NewGuid(),
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
            .Setup(s => s.BuildAuthorizationUrl(invitationId, It.IsAny<string>(), "verified@test.com"))
            .Returns("https://login.example/signin");

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["EntraExternalId:RedirectUri"] = "https://localhost/callback"
            })
            .Build();

        var handler = new StartInvitationQueryHandler(
            new UnitOfWork(context),
            tokenService,
            entraMock.Object,
            new DateTimeProvider(),
            configuration);

        var result = await handler.Handle(new StartInvitationQuery(plain), CancellationToken.None);

        result.Success.Should().BeTrue();
        result.RedirectUrl.Should().StartWith("https://login.example/signin");
    }
}
