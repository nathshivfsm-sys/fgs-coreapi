using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Features.Roles.Commands.CreateFgsRole;
using Fgs.User.Application.Features.Roles.Commands.PatchFgsRole;
using Fgs.User.Application.Features.Roles.Commands.UpdateFgsRole;
using Fgs.User.Application.Features.Roles.Dtos;
using Fgs.User.Domain.Entities;
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

        var readRepo = readRepository ?? CreateDefaultReadRepository(context, tenantAccessor);
        var unitOfWork = new EfUnitOfWork<FgsUserDbContext>(context);

        return new FgsRoleWriteService(
            context,
            unitOfWork,
            tenantAccessor,
            readRepo,
            userContext.Object);
    }

    private static async Task<FgsUserDbContext> CreateContextAsync()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

        return await TestDbContextFactory.CreateAndInitializeAsync(accessor);
    }

    private static Fgs.User.Application.Abstractions.Roles.IFgsRoleReadRepository CreateDefaultReadRepository(
        FgsUserDbContext context,
        TestTenantContextAccessor tenantAccessor)
    {
        var mock = new Mock<Fgs.User.Application.Abstractions.Roles.IFgsRoleReadRepository>();
        mock
            .Setup(r => r.HasActiveUserAssignmentsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        return mock.Object;
    }
}
