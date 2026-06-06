using System.Text.Json;
using Fgs.Contracts.Api;
using Fgs.User.Application.Common;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.Messaging.Options;
using Fgs.User.Infrastructure.Common.Options;
using Fgs.Messaging.Abstractions;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Application.Features.Auth.Commands.EntraCallback;
using Fgs.User.Application.Features.Signup;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Clients;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Messaging;
using Fgs.Persistence.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class EntraCallbackCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithMismatchedEmail_ReturnsBadRequest()
    {
        var (handler, invitationId) = await CreateHandlerAsync("admin@test.com", entraEmail: "other@test.com");
        var response = await handler.Handle(
            new EntraCallbackCommand("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
    }

    [Fact]
    public async Task Handle_WithInvalidState_ReturnsBadRequest()
    {
        var (handler, _) = await CreateHandlerAsync("admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackCommand("code", "not-a-guid"),
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

        var handler = CreateHandlerForContext(
            context,
            "admin@test.com",
            entraEmail: "admin@test.com",
            entraMock.Object);

        var response = await handler.Handle(
            new EntraCallbackCommand("code", invitationId.ToString()),
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
            new EntraCallbackCommand("code", Guid.NewGuid().ToString()),
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

        var handler = CreateHandlerForContext(context, "admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackCommand("code", invitationId.ToString()),
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

        var handler = CreateHandlerForContext(context, "admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackCommand("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.AccessToken.Should().NotBeNullOrWhiteSpace();
        response.Data.RedirectUrl.Should().Be("https://localhost/dashboard");
    }

    [Fact]
    public async Task Handle_WithValidFlow_AcceptsInvitationAndPublishesProvisionEventWithoutBusinessTypeIds()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = await SeedInvitationAsync(
            context,
            "admin@test.com",
            InvitationStatus.Pending,
            DateTimeOffset.UtcNow.AddDays(1));

        var handler = CreateHandlerForContext(context, "admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackCommand("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.AccessToken.Should().NotBeNullOrWhiteSpace();

        var outbox = await context.TenantOutboxMessages.SingleAsync();
        outbox.RoutingKey.Should().Be(IntegrationEventRoutingKeys.TenantProvisionRequested);
        outbox.Payload.Contains("businessTypeIds", StringComparison.OrdinalIgnoreCase).Should().BeFalse();

        var tenant = await context.FgsTenants.SingleAsync();
        outbox.Payload.Should().Contain($"\"tenantId\":{tenant.Id}");
        outbox.Payload.Should().Contain("\"companyId\":1");
    }

    [Fact]
    public async Task Handle_WithFgsRole_IncludesRoleInToken()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = await SeedInvitationWithFgsRoleAsync(context, "admin@test.com");

        var handler = CreateHandlerForContext(context, "admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackCommand("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Handle_WhenUserMissing_ReturnsInternalServerError()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = await SeedInvitationAsync(
            context,
            "admin@test.com",
            InvitationStatus.Pending,
            DateTimeOffset.UtcNow.AddDays(1));

        var orphanUserId = Guid.NewGuid();
        var invitation = await context.FgsInvitations.SingleAsync(i => i.Id == invitationId);
        invitation.UserId = orphanUserId;
        await context.SaveChangesAsync();

        var handler = CreateHandlerForContext(context, "admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackCommand("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.InternalServerError);
        response.Errors.Should().Contain(AuthErrorMessages.FinalizeOnboardingFailed);
    }

    [Fact]
    public async Task Handle_WhenTenantAlreadyActive_SkipsProvisionOutbox()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = await SeedInvitationAsync(
            context,
            "admin@test.com",
            InvitationStatus.Pending,
            DateTimeOffset.UtcNow.AddDays(1),
            tenantStatusId: TenantStatusIds.Active);

        var handler = CreateHandlerForContext(context, "admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackCommand("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        (await context.TenantOutboxMessages.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Handle_WhenTenantMissing_ReturnsInternalServerError()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = await SeedInvitationAsync(
            context,
            "admin@test.com",
            InvitationStatus.Pending,
            DateTimeOffset.UtcNow.AddDays(1));

        var invitation = await context.FgsInvitations.SingleAsync(i => i.Id == invitationId);
        invitation.TenantId = 99_999;
        await context.SaveChangesAsync();

        var handler = CreateHandlerForContext(context, "admin@test.com", entraEmail: "admin@test.com");
        var response = await handler.Handle(
            new EntraCallbackCommand("code", invitationId.ToString()),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.InternalServerError);
        response.Errors.Should().Contain(AuthErrorMessages.FinalizeOnboardingFailed);
    }

    private static async Task<(EntraCallbackCommandHandler Handler, Guid InvitationId)> CreateHandlerAsync(
        string inviteEmail,
        string entraEmail)
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var invitationId = await SeedInvitationAsync(
            context,
            inviteEmail,
            InvitationStatus.Pending,
            DateTimeOffset.UtcNow.AddDays(1));
        var handler = CreateHandlerForContext(context, inviteEmail, entraEmail);
        return (handler, invitationId);
    }

    private static EntraCallbackCommandHandler CreateHandlerForContext(
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
                [ConfigurationKeys.Application.DashboardUrl] = "https://localhost/dashboard"
            })
            .Build();

        IOutboxWriter outboxWriter = new OutboxWriter(
            context,
            new DateTimeProvider(),
            Options.Create(new OutboxOptions()));

        return new EntraCallbackCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            entraMock,
            new EmailNormalizer(),
            new DateTimeProvider(),
            configuration,
            outboxWriter);
    }

    private static Mock<IEntraExternalIdService> CreateEntraMock(string entraEmail)
    {
        var entraMock = new Mock<IEntraExternalIdService>();
        entraMock
            .Setup(s => s.ExchangeCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EntraTokenResult("entra-access-token", "oid-123", entraEmail, "Admin"));
        return entraMock;
    }

    private static async Task<Guid> SeedInvitationAsync(
        FgsUserDbContext context,
        string inviteEmail,
        InvitationStatus status,
        DateTimeOffset expiresAtUtc,
        Guid? invitationId = null,
        short? tenantStatusId = null)
    {
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var id = invitationId ?? Guid.NewGuid();

        var tenant = new FgsTenant
        {
            TenantCode = "t1",
            Name = "Tenant",
            FgsTenantStatusId = tenantStatusId ?? TenantStatusIds.Pending,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsTenants.Add(tenant);
        await context.SaveChangesAsync();

        var tenantId = tenant.Id;
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
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsUserRoles.Add(new FgsUserRole
        {
            UserId = userId,
            TenantId = tenantId,
            CompanyId = 1,
            GloRoleId = SignupConstants.TenantAdminGloRoleId,
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

    private static async Task<Guid> SeedInvitationWithFgsRoleAsync(
        FgsUserDbContext context,
        string inviteEmail)
    {
        var companyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var invitationId = Guid.NewGuid();

        var tenant = new FgsTenant
        {
            TenantCode = "t-fgs-role",
            Name = "Tenant",
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsTenants.Add(tenant);
        await context.SaveChangesAsync();

        var tenantId = tenant.Id;
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
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsRoles.Add(new FgsRole
        {
            Id = 1,
            TenantId = tenantId,
            CompanyId = 1,
            RoleCode = "COMPANY_ADMIN",
            Name = "Company Admin",
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsUserRoles.Add(new FgsUserRole
        {
            UserId = userId,
            TenantId = tenantId,
            CompanyId = 1,
            FgsRoleId = 1,
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsInvitations.Add(new FgsInvitation
        {
            Id = invitationId,
            UserId = userId,
            TenantId = tenantId,
            Email = inviteEmail,
            TokenHash = "hash",
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(1),
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();
        return invitationId;
    }
}
