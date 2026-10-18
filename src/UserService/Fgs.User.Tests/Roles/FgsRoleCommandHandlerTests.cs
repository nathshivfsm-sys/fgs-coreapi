using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Features.Roles.Commands.CloneFgsRole;
using Fgs.User.Application.Features.Roles.Commands.CreateFgsRole;
using Fgs.User.Application.Features.Roles.Commands.PatchFgsRole;
using Fgs.User.Application.Features.Roles.Commands.UpdateFgsRole;
using Fgs.User.Application.Features.Roles.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.Security.UserAuth;
using Fgs.User.Infrastructure.Common.Auth;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.User.Tests.Roles;

public sealed class FgsRoleCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesRole()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var handler = new CreateFgsRoleCommandHandler(
            writeService,
            NullLogger<CreateFgsRoleCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsRoleCommand(new FgsRoleCreateDto("DISPATCHER", "Dispatcher", "Schedules work", null, 2)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.RoleCode.Should().Be("DISPATCHER");
        response.Data.IsBuiltIn.Should().BeFalse();
        response.Data.DisplayOrder.Should().Be(2);
        response.Data.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task CloneHandler_CopiesPermissionsMenusAndDataAccess()
    {
        await using var context = await CreateContextAsync();
        var source = new FgsRole
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            RoleCode = "SOURCE",
            Name = "Source Role",
            Description = "Source description",
            DisplayOrder = 5,
            IsBuiltIn = true,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        };
        await context.FgsRoles.AddAsync(source);
        await context.SaveChangesAsync();

        var permission = new FgsPermission
        {
            PermissionCode = "USER.VIEW",
            Module = "User",
            Resource = "User",
            Action = "View",
            Name = "View User",
            DisplayOrder = 1,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        await context.FgsPermissions.AddAsync(permission);
        await context.SaveChangesAsync();

        await context.FgsRolePermissions.AddAsync(new FgsRolePermission
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            FgsRoleId = source.Id,
            FgsPermissionId = permission.Id,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        });
        await context.FgsRoleMenus.AddAsync(new FgsRoleMenu
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            RoleId = source.Id,
            MenuId = 42,
            DisplayOrder = 1,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        });

        var dataAccess = new FgsDataAccess
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            DataAccessCode = "OWN",
            Name = "Own",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        };
        await context.FgsDataAccesses.AddAsync(dataAccess);
        await context.SaveChangesAsync();

        await context.FgsRoleDataAccesses.AddAsync(new FgsRoleDataAccess
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            FgsRoleId = source.Id,
            FgsDataAccessId = dataAccess.Id,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        });
        await context.SaveChangesAsync();

        var writeService = CreateWriteService(context);
        var handler = new CloneFgsRoleCommandHandler(
            writeService,
            NullLogger<CloneFgsRoleCommandHandler>.Instance);

        var response = await handler.Handle(
            new CloneFgsRoleCommand(
                source.Id,
                new FgsRoleCloneDto("SOURCE_COPY", "Source Role Copy")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.RoleCode.Should().Be("SOURCE_COPY");
        response.Data.Name.Should().Be("Source Role Copy");
        response.Data.ParentRoleId.Should().Be(source.Id);
        response.Data.IsBuiltIn.Should().BeFalse();
        response.Data.Description.Should().Be("Source description");
        response.Data.DisplayOrder.Should().Be(5);

        var clonedId = response.Data.Id;
        (await context.FgsRolePermissions.CountAsync(x => x.FgsRoleId == clonedId)).Should().Be(1);
        (await context.FgsRoleMenus.CountAsync(x => x.RoleId == clonedId)).Should().Be(1);
        (await context.FgsRoleDataAccesses.CountAsync(x => x.FgsRoleId == clonedId)).Should().Be(1);
    }

    [Fact]
    public async Task CloneHandler_WhenSourceMissing_ThrowsNotFound()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);

        var act = async () => await writeService.CloneAsync(
            999,
            new FgsRoleCloneDto("MISSING_COPY", "Missing Copy"),
            CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }

    [Fact]
    public async Task CloneHandler_CopiesSubsetAndLeavesSourceUnchanged()
    {
        await using var context = await CreateContextAsync();
        var source = await SeedRoleAsync(context, "SOURCE", "Source Role", isBuiltIn: true);
        var granted = await SeedPermissionAsync(context, "USER.VIEW");
        var withheld = await SeedPermissionAsync(context, "USER.EDIT");
        await context.FgsRolePermissions.AddAsync(new FgsRolePermission
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            FgsRoleId = source.Id,
            FgsPermissionId = granted.Id,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        });
        await context.FgsRolePermissions.AddAsync(new FgsRolePermission
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            FgsRoleId = source.Id,
            FgsPermissionId = withheld.Id,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        });
        await context.FgsRoleMenus.AddAsync(new FgsRoleMenu
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            RoleId = source.Id,
            MenuId = 42,
            DisplayOrder = 1,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        });
        var dataAccess = new FgsDataAccess
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            DataAccessCode = "OWN",
            Name = "Own",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        };
        await context.FgsDataAccesses.AddAsync(dataAccess);
        await context.SaveChangesAsync();
        await context.FgsRoleDataAccesses.AddAsync(new FgsRoleDataAccess
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            FgsRoleId = source.Id,
            FgsDataAccessId = dataAccess.Id,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        });
        await context.SaveChangesAsync();

        var sourcePermissionIds = await context.FgsRolePermissions
            .Where(x => x.FgsRoleId == source.Id)
            .Select(x => x.FgsPermissionId)
            .ToListAsync();

        var writeService = CreateWriteService(context);
        var cloned = await writeService.CloneAsync(
            source.Id,
            new FgsRoleCloneDto("SOURCE_COPY", "Source Copy", null, null, [granted.Id]),
            CancellationToken.None);

        cloned.ParentRoleId.Should().Be(source.Id);
        cloned.IsBuiltIn.Should().BeFalse();
        (await context.FgsRolePermissions
            .Where(x => x.FgsRoleId == cloned.Id)
            .Select(x => x.FgsPermissionId)
            .ToListAsync()).Should().Equal(granted.Id);
        (await context.FgsRoleMenus.CountAsync(x => x.RoleId == cloned.Id)).Should().Be(1);
        (await context.FgsRoleDataAccesses.CountAsync(x => x.FgsRoleId == cloned.Id)).Should().Be(1);

        var sourceAfter = await context.FgsRoles.SingleAsync(x => x.Id == source.Id);
        sourceAfter.Name.Should().Be("Source Role");
        sourceAfter.RoleCode.Should().Be("SOURCE");
        sourceAfter.IsBuiltIn.Should().BeTrue();
        sourceAfter.ParentRoleId.Should().BeNull();
        (await context.FgsRolePermissions
            .Where(x => x.FgsRoleId == source.Id)
            .Select(x => x.FgsPermissionId)
            .ToListAsync()).Should().BeEquivalentTo(sourcePermissionIds);
        (await context.FgsRoleMenus.CountAsync(x => x.RoleId == source.Id)).Should().Be(1);
        (await context.FgsRoleDataAccesses.CountAsync(x => x.FgsRoleId == source.Id)).Should().Be(1);
    }

    [Fact]
    public async Task CloneHandler_AppliesCloneNameRule()
    {
        await using var context = await CreateContextAsync();
        var source = await SeedRoleAsync(context, "MANAGER", "Manager", isBuiltIn: true);
        var writeService = CreateWriteService(context);

        var sameName = await writeService.CloneAsync(
            source.Id,
            new FgsRoleCloneDto("COPY_A", "Manager"),
            CancellationToken.None);
        var trimmedName = await writeService.CloneAsync(
            source.Id,
            new FgsRoleCloneDto("COPY_B", " manager "),
            CancellationToken.None);
        var otherName = await writeService.CloneAsync(
            source.Id,
            new FgsRoleCloneDto("FIELD_LEAD", "Field Lead"),
            CancellationToken.None);

        sameName.Name.Should().Be("Copy of Manager");
        trimmedName.Name.Should().Be("Copy of Manager");
        otherName.Name.Should().Be("Field Lead");
        (await context.FgsRoles.SingleAsync(x => x.Id == source.Id)).Name.Should().Be("Manager");
    }

    [Fact]
    public async Task CloneHandler_RejectsCustomRoleSource()
    {
        await using var context = await CreateContextAsync();
        var source = await SeedRoleAsync(context, "CUSTOM", "Custom", isBuiltIn: false);
        var writeService = CreateWriteService(context);

        var act = async () => await writeService.CloneAsync(
            source.Id,
            new FgsRoleCloneDto("CUSTOM_COPY", "Custom Copy"),
            CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*built-in*");
        (await context.FgsRoles.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task CloneHandler_RejectsPermissionNotOnSource()
    {
        await using var context = await CreateContextAsync();
        var source = await SeedRoleAsync(context, "SOURCE", "Source Role", isBuiltIn: true);
        var granted = await SeedPermissionAsync(context, "USER.VIEW");
        var outside = await SeedPermissionAsync(context, "USER.EDIT");
        await context.FgsRolePermissions.AddAsync(new FgsRolePermission
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            FgsRoleId = source.Id,
            FgsPermissionId = granted.Id,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        });
        await context.SaveChangesAsync();
        var writeService = CreateWriteService(context);

        var act = async () => await writeService.CloneAsync(
            source.Id,
            new FgsRoleCloneDto("SOURCE_COPY", "Source Copy", null, null, [outside.Id]),
            CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"*{outside.Id}*");
        (await context.FgsRoles.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task CloneHandler_UnknownPermission_ThrowsNotFound()
    {
        await using var context = await CreateContextAsync();
        var source = await SeedRoleAsync(context, "SOURCE", "Source Role", isBuiltIn: true);
        var writeService = CreateWriteService(context);

        var act = async () => await writeService.CloneAsync(
            source.Id,
            new FgsRoleCloneDto("SOURCE_COPY", "Source Copy", null, null, [999]),
            CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }

    [Fact]
    public async Task UpdateHandler_UpdatesMutableFields()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var createHandler = new CreateFgsRoleCommandHandler(
            writeService,
            NullLogger<CreateFgsRoleCommandHandler>.Instance);
        var updateHandler = new UpdateFgsRoleCommandHandler(
            writeService,
            NullLogger<UpdateFgsRoleCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsRoleCommand(new FgsRoleCreateDto("CSR", "CSR", null)),
            CancellationToken.None);

        var response = await updateHandler.Handle(
            new UpdateFgsRoleCommand(
                created.Data!.Id,
                new FgsRoleUpdateDto("CSR", "Customer Service", "Updated description", 3)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("Customer Service");
        response.Data.Description.Should().Be("Updated description");
        response.Data.DisplayOrder.Should().Be(3);
    }

    [Fact]
    public async Task PatchHandler_DeactivatesRole()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var createHandler = new CreateFgsRoleCommandHandler(
            writeService,
            NullLogger<CreateFgsRoleCommandHandler>.Instance);
        var patchHandler = new PatchFgsRoleCommandHandler(
            writeService,
            NullLogger<PatchFgsRoleCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsRoleCommand(new FgsRoleCreateDto("BILLING", "Billing", null)),
            CancellationToken.None);

        var response = await patchHandler.Handle(
            new PatchFgsRoleCommand(created.Data!.Id, new FgsRolePatchDto(null, null, null, null, false)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task PatchHandler_BlocksDeactivationWhenUsersAssigned()
    {
        await using var context = await CreateContextAsync();
        var role = new FgsRole
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            RoleCode = "FIELD_TECH",
            Name = "Field Tech",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        await context.FgsRoles.AddAsync(role);
        await context.SaveChangesAsync();

        var readRepository = new Mock<Fgs.User.Application.Abstractions.Roles.IFgsRoleReadRepository>();
        readRepository
            .Setup(r => r.HasActiveUserAssignmentsAsync(role.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var writeService = CreateWriteService(context, readRepository.Object);

        var act = async () => await writeService.PatchAsync(
            role.Id,
            new FgsRolePatchDto(null, null, null, null, false),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*assigned*");
    }

    [Fact]
    public async Task PatchHandler_BlocksBuiltInRoleMutation()
    {
        await using var context = await CreateContextAsync();
        var role = new FgsRole
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            RoleCode = "TENANT_ADMIN",
            Name = "Tenant Admin",
            IsBuiltIn = true,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        await context.FgsRoles.AddAsync(role);
        await context.SaveChangesAsync();

        var writeService = CreateWriteService(context);

        var act = async () => await writeService.PatchAsync(
            role.Id,
            new FgsRolePatchDto(null, "Changed Name", null, null, null),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Built-in*");
    }

    private static FgsRoleWriteService CreateWriteService(
        FgsUserDbContext context,
        Fgs.User.Application.Abstractions.Roles.IFgsRoleReadRepository? readRepository = null)
    {
        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.Email).Returns("admin@test.com");

        var readRepo = readRepository ?? CreateDefaultReadRepository();
        var unitOfWork = new EfUnitOfWork<FgsUserDbContext>(context);

        var profileStore = new Mock<IUserAuthProfileStore>();
        profileStore
            .Setup(s => s.InvalidateAsync(It.IsAny<Guid>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return new FgsRoleWriteService(
            context,
            unitOfWork,
            tenantAccessor,
            readRepo,
            userContext.Object,
            new UserAuthProfileInvalidator(context, profileStore.Object));
    }

    private static async Task<FgsUserDbContext> CreateContextAsync()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

        return await TestDbContextFactory.CreateAndInitializeAsync(accessor);
    }

    private static async Task<FgsRole> SeedRoleAsync(
        FgsUserDbContext context,
        string roleCode,
        string name,
        bool isBuiltIn)
    {
        var role = new FgsRole
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            RoleCode = roleCode,
            Name = name,
            IsBuiltIn = isBuiltIn,
            IsActive = true,
            DisplayOrder = 1,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = "seed"
        };
        await context.FgsRoles.AddAsync(role);
        await context.SaveChangesAsync();
        return role;
    }

    private static async Task<FgsPermission> SeedPermissionAsync(FgsUserDbContext context, string code)
    {
        var permission = new FgsPermission
        {
            PermissionCode = code,
            Module = "User",
            Resource = "User",
            Action = "View",
            Name = code,
            DisplayOrder = 1,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        await context.FgsPermissions.AddAsync(permission);
        await context.SaveChangesAsync();
        return permission;
    }

    private static Fgs.User.Application.Abstractions.Roles.IFgsRoleReadRepository CreateDefaultReadRepository()
    {
        var mock = new Mock<Fgs.User.Application.Abstractions.Roles.IFgsRoleReadRepository>();
        mock
            .Setup(r => r.HasActiveUserAssignmentsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        return mock.Object;
    }
}
