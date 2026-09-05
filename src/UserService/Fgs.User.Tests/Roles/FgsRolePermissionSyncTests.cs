using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Features.RolePermissions.Commands.SyncFgsRolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Entities.RolePermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Roles;

public sealed class FgsRolePermissionSyncTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task Sync_AddsKeepsAndRemovesPermissions()
    {
        await using var context = await CreateContextAsync();
        var (roleId, p1, p2, p3) = await SeedRoleAndPermissionsAsync(context);
        var write = CreateWriteService(context);
        var syncHandler = new SyncFgsRolePermissionsCommandHandler(
            write,
            NullLogger<SyncFgsRolePermissionsCommandHandler>.Instance);

        var first = await syncHandler.Handle(
            new SyncFgsRolePermissionsCommand(new FgsRolePermissionSyncDto(roleId, [p1, p2])),
            CancellationToken.None);
        first.Success.Should().BeTrue();
        first.Data.Should().HaveCount(2);

        var second = await syncHandler.Handle(
            new SyncFgsRolePermissionsCommand(new FgsRolePermissionSyncDto(roleId, [p2, p3])),
            CancellationToken.None);
        second.Success.Should().BeTrue();
        second.Data!.Select(x => x.FgsPermissionId).Should().BeEquivalentTo([p2, p3]);

        var remaining = await context.FgsRolePermissions
            .Where(x => x.FgsRoleId == roleId)
            .Select(x => x.FgsPermissionId)
            .ToListAsync();
        remaining.Should().BeEquivalentTo([p2, p3]);
    }

    private static FgsRolePermissionWriteService CreateWriteService(FgsUserDbContext context)
    {
        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.Email).Returns("test@example.com");
        return new FgsRolePermissionWriteService(
            context,
            new EfUnitOfWork<FgsUserDbContext>(context),
            tenantAccessor,
            userContext.Object);
    }

    private static async Task<(long RoleId, long P1, long P2, long P3)> SeedRoleAndPermissionsAsync(
        FgsUserDbContext context)
    {
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
        var permissions = new[]
        {
            new FgsPermission
            {
                PermissionCode = "A",
                Module = "User",
                Resource = "Role",
                Action = "Read",
                Name = "A",
                CreatedOn = DateTimeOffset.UtcNow
            },
            new FgsPermission
            {
                PermissionCode = "B",
                Module = "User",
                Resource = "Role",
                Action = "Write",
                Name = "B",
                CreatedOn = DateTimeOffset.UtcNow
            },
            new FgsPermission
            {
                PermissionCode = "C",
                Module = "User",
                Resource = "Role",
                Action = "Delete",
                Name = "C",
                CreatedOn = DateTimeOffset.UtcNow
            }
        };
        context.FgsPermissions.AddRange(permissions);
        await context.SaveChangesAsync();
        return (role.Id, permissions[0].Id, permissions[1].Id, permissions[2].Id);
    }

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
