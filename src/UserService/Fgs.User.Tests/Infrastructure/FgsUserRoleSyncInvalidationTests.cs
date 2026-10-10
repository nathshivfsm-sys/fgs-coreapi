using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Features.UserRoles.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common.Auth;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Entities.UserRoles;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Fgs.User.Tests.Infrastructure;

public sealed class FgsUserRoleSyncInvalidationTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task Sync_InvalidatesAuthProfileForThatUser()
    {
        await using var context = await CreateContextAsync();
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var role = new FgsRole
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            RoleCode = "TECH",
            Name = "Technician",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        };
        context.FgsRoles.Add(role);
        context.FgsUsers.AddRange(
            CreateUser(userId, "oid-user"),
            CreateUser(otherUserId, "oid-other"));
        await context.SaveChangesAsync();

        var store = new Mock<IUserAuthProfileStore>();
        store
            .Setup(s => s.InvalidateAsync(It.IsAny<Guid>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var write = CreateWriteService(context, store.Object);

        await write.SyncAsync(new FgsUserRoleSyncDto(userId, [role.Id]), CancellationToken.None);

        store.Verify(s => s.InvalidateAsync(userId, "oid-user", It.IsAny<CancellationToken>()), Times.Once);
        store.Verify(
            s => s.InvalidateAsync(otherUserId, It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static FgsUserRoleWriteService CreateWriteService(
        FgsUserDbContext context,
        IUserAuthProfileStore profileStore)
    {
        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.Email).Returns("test@example.com");
        return new FgsUserRoleWriteService(
            context,
            new EfUnitOfWork<FgsUserDbContext>(context),
            tenantAccessor,
            userContext.Object,
            new UserAuthProfileInvalidator(context, profileStore));
    }

    private static FgsUser CreateUser(Guid id, string entraObjectId) =>
        new()
        {
            Id = id,
            TenantId = TenantId,
            CompanyId = CompanyId,
            Email = $"{id:N}@example.com",
            DisplayName = "User",
            EntraObjectId = entraObjectId,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "test"
        };

    private static async Task<FgsUserDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsUserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        var context = new FgsUserDbContext(
            options,
            new TestTenantContextAccessor
            {
                Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
            });
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
