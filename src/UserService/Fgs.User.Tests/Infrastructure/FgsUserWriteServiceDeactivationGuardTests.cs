using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Security.Constants;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Invitations;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Users.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Common.Auth;
using Fgs.User.Infrastructure.Entities.UserRoles;
using Fgs.User.Infrastructure.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Fgs.User.Tests.Infrastructure;

public sealed class FgsUserWriteServiceDeactivationGuardTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    private const string TenantAdminBlockedMessage = "Tenant administrator accounts cannot be deactivated.";

    private const string CompanyAdminBlockedMessage =
        "Company administrator accounts can only be deactivated by a tenant administrator.";

    [Fact]
    public async Task TenantAdmin_StaysActive_OnUpdatePatchAndSetAccess_WhenActorIsTenantAdmin()
    {
        await using var context = await CreateContextAsync();
        var seeded = await SeedUserAsync(context, FgsRoleCodes.TenantAdmin);
        var write = CreateWriteService(context, actorIsTenantAdmin: true);

        var renamed = await write.UpdateAsync(
            seeded.UserId,
            new FgsUserUpdateDto("Renamed Admin", "555-0100", [seeded.RoleId], true));
        renamed.DisplayName.Should().Be("Renamed Admin");
        renamed.IsActive.Should().BeTrue();

        await AssertDeactivationRejectedAsync(
            write,
            context,
            seeded,
            TenantAdminBlockedMessage);
    }

    [Fact]
    public async Task CompanyAdmin_StaysActive_WhenActorIsNotTenantAdmin()
    {
        await using var context = await CreateContextAsync();
        var seeded = await SeedUserAsync(context, FgsRoleCodes.CompanyAdmin);
        var write = CreateWriteService(context, actorIsTenantAdmin: false);

        await AssertDeactivationRejectedAsync(
            write,
            context,
            seeded,
            CompanyAdminBlockedMessage);
    }

    [Fact]
    public async Task CompanyAdmin_StaysActive_WhenActorHasNoRoles()
    {
        await using var context = await CreateContextAsync();
        var seeded = await SeedUserAsync(context, FgsRoleCodes.CompanyAdmin);
        var write = CreateWriteService(context, actorIsTenantAdmin: false);

        await AssertDeactivationRejectedAsync(
            write,
            context,
            seeded,
            CompanyAdminBlockedMessage);
    }

    [Fact]
    public async Task UserWithBothAdminRoles_StaysActive_WhenActorIsTenantAdmin()
    {
        await using var context = await CreateContextAsync();
        var seeded = await SeedUserAsync(context, FgsRoleCodes.TenantAdmin, FgsRoleCodes.CompanyAdmin);
        var write = CreateWriteService(context, actorIsTenantAdmin: true);

        await AssertDeactivationRejectedAsync(
            write,
            context,
            seeded,
            TenantAdminBlockedMessage);
    }

    [Fact]
    public async Task CompanyAdmin_BecomesInactive_WhenActorIsTenantAdmin()
    {
        await using var context = await CreateContextAsync();
        var seeded = await SeedUserAsync(context, FgsRoleCodes.CompanyAdmin);
        var write = CreateWriteService(context, actorIsTenantAdmin: true);

        var updated = await write.UpdateAsync(
            seeded.UserId,
            new FgsUserUpdateDto("Company Admin", null, [seeded.RoleId], false));
        updated.IsActive.Should().BeFalse();
        (await ReloadAsync(context, seeded.UserId)).IsActive.Should().BeFalse();

        await write.SetAccessAsync(seeded.UserId, true);
        (await ReloadAsync(context, seeded.UserId)).IsActive.Should().BeTrue();

        var patched = await write.PatchAsync(
            seeded.UserId,
            new FgsUserPatchDto(null, null, null, false));
        patched.IsActive.Should().BeFalse();

        await write.SetAccessAsync(seeded.UserId, true);
        await write.SetAccessAsync(seeded.UserId, false);
        (await ReloadAsync(context, seeded.UserId)).IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task NonAdmin_CanBeDeactivated_AndCompanyAdmin_CanBeReactivatedWithoutTenantAdminRole()
    {
        await using var context = await CreateContextAsync();
        var technician = await SeedUserAsync(context, "TECH");
        var companyAdmin = await SeedUserAsync(context, FgsRoleCodes.CompanyAdmin, isActive: false);
        var write = CreateWriteService(context, actorIsTenantAdmin: false);

        var updated = await write.UpdateAsync(
            technician.UserId,
            new FgsUserUpdateDto("Technician", null, [technician.RoleId], false));
        updated.IsActive.Should().BeFalse();
        (await ReloadAsync(context, technician.UserId)).IsActive.Should().BeFalse();

        await write.SetAccessAsync(companyAdmin.UserId, true);
        (await ReloadAsync(context, companyAdmin.UserId)).IsActive.Should().BeTrue();

        var edited = await write.UpdateAsync(
            companyAdmin.UserId,
            new FgsUserUpdateDto("Company Admin", "555-0199", [companyAdmin.RoleId], true));
        edited.IsActive.Should().BeTrue();
        edited.DisplayName.Should().Be("Company Admin");
        edited.PhoneNumber.Should().Be("555-0199");
    }

    private static async Task AssertDeactivationRejectedAsync(
        IFgsUserWriteService write,
        FgsUserDbContext context,
        SeededUser seeded,
        string expectedMessage)
    {
        var update = () => write.UpdateAsync(
            seeded.UserId,
            new FgsUserUpdateDto("Should Not Save", null, [seeded.RoleId], false));
        await update.Should().ThrowAsync<InvalidOperationException>().WithMessage(expectedMessage);

        var patch = () => write.PatchAsync(
            seeded.UserId,
            new FgsUserPatchDto(null, null, null, false));
        await patch.Should().ThrowAsync<InvalidOperationException>().WithMessage(expectedMessage);

        var access = () => write.SetAccessAsync(seeded.UserId, false);
        await access.Should().ThrowAsync<InvalidOperationException>().WithMessage(expectedMessage);

        var user = await ReloadAsync(context, seeded.UserId);
        user.IsActive.Should().BeTrue();
        user.DisplayName.Should().NotBe("Should Not Save");
    }

    private static FgsUserWriteService CreateWriteService(FgsUserDbContext context, bool actorIsTenantAdmin)
    {
        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.Email).Returns("admin@example.com");
        userContext.SetupGet(x => x.DisplayName).Returns("Admin");
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        userContext.Setup(x => x.IsInRole(It.IsAny<string>()))
            .Returns<string>(roleCode =>
                actorIsTenantAdmin
                && string.Equals(roleCode, FgsRoleCodes.TenantAdmin, StringComparison.OrdinalIgnoreCase));

        var unitOfWork = new EfUnitOfWork<FgsUserDbContext>(context);
        var roleProfileStore = new Mock<IUserAuthProfileStore>();
        roleProfileStore
            .Setup(store => store.InvalidateAsync(It.IsAny<Guid>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var userRoleWrite = new FgsUserRoleWriteService(
            context,
            unitOfWork,
            tenantAccessor,
            userContext.Object,
            new UserAuthProfileInvalidator(context, roleProfileStore.Object));
        var profileStore = new Mock<IUserAuthProfileStore>();
        profileStore
            .Setup(store => store.InvalidateAsync(It.IsAny<Guid>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return new FgsUserWriteService(
            context,
            unitOfWork,
            tenantAccessor,
            userContext.Object,
            new InMemoryFgsUserReadRepository(context),
            userRoleWrite,
            Mock.Of<IUserInvitationIssuer>(),
            profileStore.Object);
    }

    private static async Task<SeededUser> SeedUserAsync(
        FgsUserDbContext context,
        string roleCode,
        string? additionalRoleCode = null,
        bool isActive = true)
    {
        var roleId = await SeedRoleAsync(context, roleCode);
        long? additionalRoleId = additionalRoleCode is null
            ? null
            : await SeedRoleAsync(context, additionalRoleCode);

        var user = new FgsUser
        {
            Id = Guid.NewGuid(),
            TenantId = TenantId,
            CompanyId = CompanyId,
            Email = $"{Guid.NewGuid():N}@example.com",
            DisplayName = roleCode,
            IsActive = isActive,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        };
        context.FgsUsers.Add(user);
        context.FgsUserRoles.Add(CreateAssignment(user.Id, roleId));
        if (additionalRoleId.HasValue)
        {
            context.FgsUserRoles.Add(CreateAssignment(user.Id, additionalRoleId.Value));
        }

        await context.SaveChangesAsync();
        return new SeededUser(user.Id, roleId);
    }

    private static FgsUserRole CreateAssignment(Guid userId, long roleId) =>
        new()
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            UserId = userId,
            FgsRoleId = roleId,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        };

    private static async Task<long> SeedRoleAsync(FgsUserDbContext context, string roleCode)
    {
        var role = new FgsRole
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            RoleCode = roleCode,
            Name = roleCode,
            IsBuiltIn = false,
            DisplayOrder = 1,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        };
        context.FgsRoles.Add(role);
        await context.SaveChangesAsync();
        return role.Id;
    }

    private static async Task<FgsUser> ReloadAsync(FgsUserDbContext context, Guid userId)
    {
        context.ChangeTracker.Clear();
        return await context.FgsUsers.SingleAsync(user => user.Id == userId);
    }

    private static async Task<FgsUserDbContext> CreateContextAsync()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };
        return await TestDbContextFactory.CreateAndInitializeAsync(accessor);
    }

    private sealed record SeededUser(Guid UserId, long RoleId);

    private sealed class InMemoryFgsUserReadRepository(FgsUserDbContext context) : IFgsUserReadRepository
    {
        public async Task<FgsUserDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await context.FgsUsers.AsNoTracking()
                .FirstOrDefaultAsync(candidate => candidate.Id == id && !candidate.IsDeleted, cancellationToken);
            return user is null
                ? null
                : new FgsUserDetailDto(
                    user.Id,
                    user.DisplayName,
                    user.Email,
                    user.PhoneNumber,
                    null,
                    null,
                    null,
                    user.IsActive,
                    false,
                    user.LastLoginOn);
        }

        public Task<FgsUserListResultDto> ListAsync(
            IdentityListQuery query,
            FgsUserListFilters filters,
            bool includeSummary = true,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<bool> ExistsByEmailAsync(
            string email,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<bool> HasAcceptedInvitationAsync(Guid userId, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<IReadOnlyList<Guid>> GetIdsByRoleIdsAsync(
            IReadOnlyList<long> roleIds,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();

        public Task<IReadOnlyList<Fgs.Contracts.Clients.FgsUserListEnrichmentDto>> GetListEnrichmentAsync(
            IReadOnlyList<Guid> userIds,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
    }
}
