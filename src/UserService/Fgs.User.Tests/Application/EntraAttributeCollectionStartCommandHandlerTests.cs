using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Features.Auth.Commands.EntraAttributeCollectionStart;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Common.Identity;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Database;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class EntraAttributeCollectionStartCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithSignupContactName_PrefillsDisplayName()
    {
        await using var context = await TestDbContextFactory.CreateAndInitializeAsync();
        await SeedUserWithInvitationAsync(context, "admin@test.com", "Jane Admin");

        var handler = CreateHandler(context);
        var response = await handler.Handle(
            new EntraAttributeCollectionStartCommand(CreateRequest("admin@test.com")),
            CancellationToken.None);

        var action = response.Data.Actions.Should().ContainSingle().Subject;
        action.ODataType.Should().Be("microsoft.graph.attributeCollectionStart.setPrefillValues");
        action.Inputs.Should().ContainKey("displayName").WhoseValue.Should().Be("Jane Admin");
    }

    [Fact]
    public async Task Handle_WithUnknownEmail_ContinuesWithoutPrefill()
    {
        await using var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var handler = CreateHandler(context);

        var response = await handler.Handle(
            new EntraAttributeCollectionStartCommand(CreateRequest("unknown@test.com")),
            CancellationToken.None);

        var action = response.Data.Actions.Should().ContainSingle().Subject;
        action.ODataType.Should().Be("microsoft.graph.attributeCollectionStart.continueWithDefaultBehavior");
        action.Inputs.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WithMixedCaseEmail_PrefillsDisplayName()
    {
        await using var context = await TestDbContextFactory.CreateAndInitializeAsync();
        await SeedUserWithInvitationAsync(context, "Fgs_User@Test.com", "Admin User");

        var handler = CreateHandler(context);
        var response = await handler.Handle(
            new EntraAttributeCollectionStartCommand(CreateRequest("fgs_user@test.com")),
            CancellationToken.None);

        var action = response.Data.Actions.Should().ContainSingle().Subject;
        action.ODataType.Should().Be("microsoft.graph.attributeCollectionStart.setPrefillValues");
        action.Inputs.Should().ContainKey("displayName").WhoseValue.Should().Be("Admin User");
    }

    [Fact]
    public async Task Handle_WithMissingEmail_ContinuesWithoutPrefill()
    {
        await using var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var handler = CreateHandler(context);

        var response = await handler.Handle(
            new EntraAttributeCollectionStartCommand(new EntraAttributeCollectionStartRequestDto()),
            CancellationToken.None);

        var action = response.Data.Actions.Should().ContainSingle().Subject;
        action.ODataType.Should().Be("microsoft.graph.attributeCollectionStart.continueWithDefaultBehavior");
    }

    private static EntraAttributeCollectionStartCommandHandler CreateHandler(FgsUserDbContext context)
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

        return new EntraAttributeCollectionStartCommandHandler(profileResolver, new EmailNormalizer());
    }

    private static EntraAttributeCollectionStartRequestDto CreateRequest(string email) =>
        new()
        {
            Type = "microsoft.graph.authenticationEvent.attributeCollectionStart",
            Data = new EntraAttributeCollectionStartRequestDataDto
            {
                UserSignUpInfo = new EntraAttributeCollectionUserSignUpInfoDto
                {
                    Identities =
                    [
                        new EntraAttributeCollectionIdentityDto
                        {
                            SignInType = "email",
                            IssuerAssignedId = email
                        }
                    ]
                }
            }
        };

    private static async Task SeedUserWithInvitationAsync(
        FgsUserDbContext context,
        string email,
        string displayName)
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
        var companyGuid = Guid.NewGuid();
        context.FgsTenantCompanies.Add(new FgsTenantCompany
        {
            TenantId = tenantId,
            CompanyNumber = companyId,
            CompanyGuid = companyGuid,
            Code = "c1",
            Name = "Company",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        context.FgsTenantCompanyCaches.Add(new FgsTenantCompanyCache
        {
            TenantId = tenantId,
            CompanyId = companyId,
            CompanyGuid = companyGuid,
            CompanyCode = "c1",
            CompanyName = "Company",
            IsActive = true,
            UpdatedOn = DateTimeOffset.UtcNow
        });
        context.FgsUsers.Add(new FgsUser
        {
            Id = userId,
            TenantId = tenantId,
            CompanyId = companyId,
            Email = email.Trim().ToUpperInvariant(),
            DisplayName = displayName,
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
    }
}
