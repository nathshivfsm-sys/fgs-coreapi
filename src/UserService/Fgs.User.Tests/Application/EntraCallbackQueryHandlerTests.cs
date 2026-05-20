using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Application.Features.Auth.Queries.EntraCallback;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Fgs.User.Infrastructure.Persistence.Database.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class EntraCallbackQueryHandlerTests
{
    [Fact]
    public async Task Handle_WithMismatchedEmail_ReturnsBadRequest()
    {
        var (handler, invitationId) = await CreateHandlerAsync("admin@test.com", entraEmail: "other@test.com");
        var response = await handler.Handle(
            new EntraCallbackQuery("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
    }

    [Fact]
    public async Task Handle_WithInvalidState_ReturnsBadRequest()
    {
        var (handler, _) = await CreateHandlerAsync("admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackQuery("code", "not-a-guid"),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.Errors.Should().Contain(AuthErrorMessages.InvalidOAuthState);
    }

    [Fact]
    public async Task Handle_WhenEntraExchangeFails_ReturnsUnauthorized()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = await SeedInvitationAsync(
            context,
            "admin@test.com",
            InvitationStatus.Pending,
            DateTimeOffset.UtcNow.AddDays(1));

        var entraMock = new Mock<IEntraExternalIdService>();
        entraMock
            .Setup(s => s.ExchangeCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("entra down"));

        var handler = await CreateHandlerForContextAsync(
            context,
            "admin@test.com",
            entraEmail: "admin@test.com",
            entraMock.Object);

        var response = await handler.Handle(
            new EntraCallbackQuery("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.Unauthorized);
        response.Errors.Should().Contain(AuthErrorMessages.EntraCodeExchangeFailed);
    }

    [Fact]
    public async Task Handle_WhenInvitationNotFound_ReturnsNotFound()
    {
        var (handler, _) = await CreateHandlerAsync("admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackQuery("code", Guid.NewGuid().ToString()),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task Handle_WhenInvitationExpired_ReturnsBadRequest()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = await SeedInvitationAsync(
            context,
            "admin@test.com",
            InvitationStatus.Pending,
            DateTimeOffset.UtcNow.AddDays(-1));

        var handler = await CreateHandlerForContextAsync(context, "admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackQuery("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.Errors.Should().Contain(AuthErrorMessages.InvitationNotActive);
    }

    [Fact]
    public async Task Handle_WhenAlreadyAccepted_ReturnsTokenWithoutUpdatingInvitation()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = await SeedInvitationAsync(
            context,
            "admin@test.com",
            InvitationStatus.Accepted,
            DateTimeOffset.UtcNow.AddDays(1));

        var handler = await CreateHandlerForContextAsync(context, "admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackQuery("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.AccessToken.Should().NotBeNullOrWhiteSpace();
        response.Data.RedirectUrl.Should().Be("https://localhost/dashboard");
    }

    [Fact]
    public async Task Handle_WithValidFlow_AcceptsInvitationAndReturnsToken()
    {
        var (handler, invitationId) = await CreateHandlerAsync("admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackQuery("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    private static async Task<(EntraCallbackQueryHandler Handler, Guid InvitationId)> CreateHandlerAsync(
        string inviteEmail,
        string entraEmail)
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = await SeedInvitationAsync(
            context,
            inviteEmail,
            InvitationStatus.Pending,
            DateTimeOffset.UtcNow.AddDays(1));
        var handler = await CreateHandlerForContextAsync(context, inviteEmail, entraEmail);
        return (handler, invitationId);
    }

    private static async Task<EntraCallbackQueryHandler> CreateHandlerForContextAsync(
        FgsUserDbContext context,
        string inviteEmail,
        string entraEmail,
        IEntraExternalIdService? entraService = null)
    {
        var entraMock = entraService ?? CreateEntraMock(entraEmail).Object;
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConfigurationKeys.EntraExternalId.RedirectUri] = "https://localhost/callback",
                [ConfigurationKeys.Application.DashboardUrl] = "https://localhost/dashboard",
                ["Jwt:Issuer"] = "test",
                ["Jwt:Audience"] = "test",
                ["Jwt:SigningKey"] = "super-secret-signing-key-32chars!!",
                ["Jwt:ExpiryMinutes"] = "60"
            })
            .Build();

        return new EntraCallbackQueryHandler(
            new UnitOfWork(context),
            entraMock,
            new JwtTokenService(Microsoft.Extensions.Options.Options.Create(
                new Fgs.User.Infrastructure.Common.Options.JwtOptions
                {
                    Issuer = "test",
                    Audience = "test",
                    SigningKey = "super-secret-signing-key-32chars!!",
                    ExpiryMinutes = 60
                })),
            new EmailNormalizer(),
            new DateTimeProvider(),
            configuration);
    }

    private static Mock<IEntraExternalIdService> CreateEntraMock(string entraEmail)
    {
        var entraMock = new Mock<IEntraExternalIdService>();
        entraMock
            .Setup(s => s.ExchangeCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EntraTokenResult("oid-123", entraEmail, "Admin"));
        return entraMock;
    }

    private static async Task<Guid> SeedInvitationAsync(
        FgsUserDbContext context,
        string inviteEmail,
        InvitationStatus status,
        DateTimeOffset expiresAtUtc,
        Guid? invitationId = null)
    {
        var tenantId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var id = invitationId ?? Guid.NewGuid();

        context.FgsTenants.Add(new FgsTenant
        {
            Id = tenantId,
            TenantCode = "t1",
            Name = "Tenant",
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsTenantCompanies.Add(new FgsTenantCompany
        {
            CompanyGuid = companyId,
            TenantId = tenantId,
            CompanyNumber = 1,
            BusinessTypeId = 1,
            Code = "c1",
            Name = "Company",
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = tenantId,
            CompanyId = 1,
            Email = inviteEmail,
            DisplayName = "Admin",
            Role = UserRoleType.Admin,
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsInvitations.Add(new FgsInvitation
        {
            Id = id,
            UserId = userId,
            TenantId = tenantId,
            Email = inviteEmail,
            TokenHash = "hash",
            Status = status,
            ExpiresAtUtc = expiresAtUtc,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();
        return id;
    }
}
