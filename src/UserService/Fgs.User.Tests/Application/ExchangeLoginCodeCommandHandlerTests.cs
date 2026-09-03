using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Time;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Options;
using Fgs.Persistence.Implementations;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Application.Features.Auth.Commands.ExchangeLoginCode;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class ExchangeLoginCodeCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidLoginCodeAndState_ReturnsLoginProfile()
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
        var handler = CreateHandler(context, entra.Object, pkceStore.Object, profileStore.Object);

        var result = await handler.Handle(new ExchangeLoginCodeCommand("code", state), CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Data!.AccessToken.Should().Be("access");
        result.Data.RefreshToken.Should().Be("refresh");
        result.Data.User.Email.Should().Be("user@test.com");
        result.Data.User.FirstName.Should().Be("Ada");
        profileStore.Verify(s => s.SetAsync(It.IsAny<Fgs.Contracts.Auth.UserAuthProfileDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidInvitationState_FinalizesInviteAndReturnsLoginProfile()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var tenant = new FgsTenant
        {
            TenantCode = "t1",
            Name = "Tenant",
            FgsTenantStatusId = TenantStatusIds.Pending,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = SignupConstants.ProspectActor
        };
        context.FgsTenants.Add(tenant);
        await context.SaveChangesAsync();

        context.FgsTenantCompanies.Add(new FgsTenantCompany
        {
            CompanyGuid = Guid.NewGuid(),
            TenantId = tenant.Id,
            CompanyNumber = 1,
            Code = "C1",
            Name = "Company",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = tenant.Id,
            CompanyId = 1,
            Email = "invite@test.com",
            DisplayName = "Invite User",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsInvitations.Add(new FgsInvitation
        {
            Id = invitationId,
            UserId = userId,
            TenantId = tenant.Id,
            Email = "invite@test.com",
            TokenHash = "hash",
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(1),
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var state = invitationId.ToString();
        var pkceStore = new Mock<ILoginPkceStore>();
        pkceStore
            .Setup(s => s.TakeAsync(state, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LoginPkceState("verifier", "https://app.local/auth/callback", userId));

        var entra = new Mock<IEntraExternalIdService>();
        entra
            .Setup(s => s.ExchangeLoginCodeAsync(
                "code",
                "https://app.local/auth/callback",
                "verifier",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EntraTokenResult(
                "access",
                "oid-invite",
                "invite@test.com",
                "Invite User",
                "refresh",
                "id",
                3600,
                "Bearer"));

        var profileStore = new Mock<IUserAuthProfileStore>();
        var handler = CreateHandler(context, entra.Object, pkceStore.Object, profileStore.Object);

        var result = await handler.Handle(new ExchangeLoginCodeCommand("code", state), CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Data!.AccessToken.Should().Be("access");
        result.Data.RefreshToken.Should().Be("refresh");

        var invitation = await context.FgsInvitations.FindAsync(invitationId);
        invitation!.Status.Should().Be(InvitationStatus.Accepted);
        var user = await context.FgsUsers.FindAsync(userId);
        user!.EntraObjectId.Should().Be("oid-invite");
        var updatedTenant = await context.FgsTenants.FindAsync(tenant.Id);
        updatedTenant!.FgsTenantStatusId.Should().Be(TenantStatusIds.Provisioning);
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

        var handler = CreateHandler(context, Mock.Of<IEntraExternalIdService>(), pkceStore.Object);

        var result = await handler.Handle(new ExchangeLoginCodeCommand("code", state), CancellationToken.None);

        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        result.Errors.Should().Contain(AuthErrorMessages.PkceStateExpired);
    }

    [Fact]
    public async Task Handle_WithInvalidState_ReturnsBadRequest()
    {
        var handler = CreateHandler(
            await TestDbContextFactory.CreateAndInitializeAsync(),
            Mock.Of<IEntraExternalIdService>(),
            Mock.Of<ILoginPkceStore>());

        var result = await handler.Handle(new ExchangeLoginCodeCommand("code", "invalid-state"), CancellationToken.None);

        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        result.Errors.Should().Contain(AuthErrorMessages.InvalidOAuthState);
    }

    [Fact]
    public async Task Handle_WhenEntraExchangeFails_ReturnsUnauthorized()
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

        var state = $"{OAuthStatePrefixes.UserLogin}{userId}";
        var pkceStore = new Mock<ILoginPkceStore>();
        pkceStore.Setup(s => s.TakeAsync(state, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LoginPkceState("verifier", "https://localhost/callback", userId));

        var entra = new Mock<IEntraExternalIdService>();
        entra.Setup(s => s.ExchangeLoginCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("exchange failed"));

        var handler = CreateHandler(context, entra.Object, pkceStore.Object);

        var result = await handler.Handle(new ExchangeLoginCodeCommand("code", state), CancellationToken.None);

        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(ApiStatusCodes.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenEmailMismatch_ReturnsBadRequest()
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

        var state = $"{OAuthStatePrefixes.UserLogin}{userId}";
        var pkceStore = new Mock<ILoginPkceStore>();
        pkceStore.Setup(s => s.TakeAsync(state, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LoginPkceState("verifier", "https://localhost/callback", userId));

        var entra = new Mock<IEntraExternalIdService>();
        entra.Setup(s => s.ExchangeLoginCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EntraTokenResult("access", "oid-123", "other@test.com", "Other", "refresh", "id", 3600, "Bearer"));

        var handler = CreateHandler(context, entra.Object, pkceStore.Object);

        var result = await handler.Handle(new ExchangeLoginCodeCommand("code", state), CancellationToken.None);

        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        result.Errors.Should().Contain(AuthErrorMessages.EntraEmailMismatch);
    }

    private static ExchangeLoginCodeCommandHandler CreateHandler(
        FgsUserDbContext context,
        IEntraExternalIdService entra,
        ILoginPkceStore pkceStore,
        IUserAuthProfileStore? profileStore = null)
    {
        IOutboxWriter outboxWriter = new OutboxWriter(
            context,
            new DateTimeProvider(),
            Options.Create(new OutboxOptions()));

        return new ExchangeLoginCodeCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            entra,
            new EmailNormalizer(),
            pkceStore,
            TestUserRepositories.ProfileBuilder(),
            profileStore ?? Mock.Of<IUserAuthProfileStore>(),
            new DateTimeProvider(),
            outboxWriter);
    }
}
