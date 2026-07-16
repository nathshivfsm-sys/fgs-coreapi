using Fgs.User.Application.Abstractions.Identity;
using Fgs.Contracts.Api;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth;
using Fgs.Persistence.Abstractions;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Application.Features.Auth.Commands.StartLogin;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Database;
using Fgs.Persistence.Implementations;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class StartLoginCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithActiveBoundUser_ReturnsEntraRedirectUrl()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var (tenantId, companyId, userId) = await SeedActiveTenantCompanyUserAsync(
            context,
            "ACTIVE@TEST.COM",
            entraObjectId: "oid-1");

        var entraMock = new Mock<IEntraExternalIdService>();
        entraMock
            .Setup(s => s.BuildLoginAuthorizationUrl(
                $"{OAuthStatePrefixes.UserLogin}{userId}",
                It.IsAny<string>(),
                It.IsAny<string>(),
                "ACTIVE@TEST.COM"))
            .Returns("https://login.example/authorize");

        var pkceStore = new Mock<ILoginPkceStore>();
        var handler = CreateHandler(context, entraMock.Object, pkceStore.Object);
        var result = await handler.Handle(new StartLoginCommand("active@test.com"), CancellationToken.None);

        result.Success.Should().BeTrue();
        result.Data!.RedirectUrl.Should().Be("https://login.example/authorize");
        pkceStore.Verify(
            s => s.SaveAsync(
                $"{OAuthStatePrefixes.UserLogin}{userId}",
                It.IsAny<LoginPkceState>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _ = tenantId;
        _ = companyId;
    }

    [Fact]
    public async Task Handle_WithInactiveUser_ReturnsUserNotActive()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        await SeedActiveTenantCompanyUserAsync(
            context,
            "INACTIVE@TEST.COM",
            entraObjectId: "oid-1",
            isActive: false);

        var result = await CreateHandler(context).Handle(
            new StartLoginCommand("inactive@test.com"),
            CancellationToken.None);

        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(ApiStatusCodes.Forbidden);
        result.Errors.Should().Contain(AuthErrorMessages.UserNotActive);
    }

    [Fact]
    public async Task Handle_WithPendingInvitationOnly_ReturnsInvitationNotAccepted()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var (tenantId, _, userId) = await SeedActiveTenantCompanyUserAsync(
            context,
            "PENDING@TEST.COM",
            entraObjectId: null);
        context.FgsInvitations.Add(new FgsInvitation
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TenantId = tenantId,
            Email = "pending@test.com",
            TokenHash = "hash",
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(1),
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();

        var result = await CreateHandler(context).Handle(
            new StartLoginCommand("pending@test.com"),
            CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(AuthErrorMessages.InvitationNotAccepted);
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
        IEntraExternalIdService? entraService = null,
        ILoginPkceStore? pkceStore = null)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConfigurationKeys.EntraExternalId.LoginRedirectUri] = "https://localhost:3000/auth/callback"
            })
            .Build();

        return new StartLoginCommandHandler(
            new EfUnitOfWork<FgsUserDbContext>(context),
            entraService ?? Mock.Of<IEntraExternalIdService>(),
            new EmailNormalizer(),
            TestUserRepositories.InvitationRead(context),
            pkceStore ?? Mock.Of<ILoginPkceStore>(),
            configuration);
    }

    private static async Task<(long TenantId, long CompanyId, Guid UserId)> SeedActiveTenantCompanyUserAsync(
        FgsUserDbContext context,
        string email,
        string? entraObjectId,
        bool isActive = true)
    {
        var tenant = new FgsTenant
        {
            TenantGuid = Guid.NewGuid(),
            TenantCode = $"t-{Guid.NewGuid():N}"[..12],
            Name = "Tenant",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsTenants.Add(tenant);
        await context.SaveChangesAsync();

        const long companyId = 1;
        context.FgsTenantCompanies.Add(new FgsTenantCompany
        {
            TenantId = tenant.Id,
            CompanyNumber = companyId,
            CompanyGuid = Guid.NewGuid(),
            Code = "c1",
            Name = "Company",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });

        var userId = Guid.NewGuid();
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = tenant.Id,
            CompanyId = companyId,
            Email = email,
            DisplayName = "User",
            EntraObjectId = entraObjectId,
            IsActive = isActive,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();
        return (tenant.Id, companyId, userId);
    }
}
