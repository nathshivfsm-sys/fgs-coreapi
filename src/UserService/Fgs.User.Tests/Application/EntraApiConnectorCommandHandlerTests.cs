using Fgs.User.Application.Features.Auth.Commands.EntraApiConnector;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Infrastructure.Common.Identity;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Database;
using Fgs.Persistence.Implementations;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class EntraApiConnectorCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithSignupEmail_ReturnsTenantAndCompanyClaims()
    {
        await using var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var (tenantId, companyId) = await SeedUserWithInvitationAsync(context, "admin@test.com");

        var handler = CreateHandler(context);
        var response = await handler.Handle(
            new EntraApiConnectorCommand("admin@test.com", null),
            CancellationToken.None);

        response.Action.Should().Be("Continue");
        response.TenantId.Should().Be(tenantId.ToString());
        response.CompanyId.Should().Be(companyId.ToString());
    }

    [Fact]
    public async Task Handle_WithUnknownEmail_ReturnsBlockPage()
    {
        await using var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var handler = CreateHandler(context);

        var response = await handler.Handle(
            new EntraApiConnectorCommand("unknown@test.com", null),
            CancellationToken.None);

        response.Action.Should().Be("ShowBlockPage");
        response.UserMessage.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Handle_WithMismatchedObjectId_ReturnsBlockPage()
    {
        await using var context = await TestDbContextFactory.CreateAndInitializeAsync();
        await SeedUserWithInvitationAsync(
            context,
            "admin@test.com",
            entraObjectId: "linked-oid");

        var handler = CreateHandler(context);
        var response = await handler.Handle(
            new EntraApiConnectorCommand("admin@test.com", "other-oid"),
            CancellationToken.None);

        response.Action.Should().Be("ShowBlockPage");
    }

    private static EntraApiConnectorCommandHandler CreateHandler(FgsUserDbContext context)
    {
        var publicEndpoints = new Mock<Fgs.User.Application.Abstractions.PublicEndpoints.IFgsPublicEndpointReadRepository>();
        publicEndpoints
            .Setup(r => r.ListActiveForTenantCompanyAsync(
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var profileResolver = new FgsUserProfileResolver(
            TestUserRepositories.ReadUsers(context),
            TestUserRepositories.InvitationRead(context),
            TestUserRepositories.RoleCodesRead(context, ["TENANT_ADMIN"]),
            TestUserRepositories.AuthorizationRead(),
            publicEndpoints.Object);

        return new EntraApiConnectorCommandHandler(profileResolver, new EmailNormalizer());
    }

    private static async Task<(long TenantId, long CompanyId)> SeedUserWithInvitationAsync(
        FgsUserDbContext context,
        string email,
        string? entraObjectId = null)
    {
        const long companyId = 1;
        var userId = Guid.NewGuid();
        var tenant = new FgsTenant
        {
            TenantGuid = Guid.NewGuid(),
            TenantCode = "t-code",
            Name = "Tenant",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsTenants.Add(tenant);
        await context.SaveChangesAsync();

        var tenantId = tenant.Id;
        context.FgsTenantCompanies.Add(new FgsTenantCompany
        {
            TenantId = tenantId,
            CompanyNumber = companyId,
            CompanyGuid = Guid.NewGuid(),
            Code = "c1",
            Name = "Company",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = tenantId,
            CompanyId = companyId,
            Email = email.Trim().ToUpperInvariant(),
            DisplayName = "Admin",
            EntraObjectId = entraObjectId,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsInvitations.Add(new FgsInvitation
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TenantId = tenantId,
            Email = email,
            TokenHash = "hash",
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(1),
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();
        return (tenantId, companyId);
    }
}
