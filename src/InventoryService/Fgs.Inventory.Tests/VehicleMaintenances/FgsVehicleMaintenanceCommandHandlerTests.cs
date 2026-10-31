using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Commands.CreateFgsVehicleMaintenance;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Commands.DeleteFgsVehicleMaintenance;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Commands.UpdateFgsVehicleMaintenance;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Dtos;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Inventory.Infrastructure.Entities.VehicleMaintenances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Inventory.Tests.VehicleMaintenances;

public sealed class FgsVehicleMaintenanceCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateHandler_CreatesActiveRecord()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsVehicleMaintenanceCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsVehicleMaintenanceCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsVehicleMaintenanceCommand(new FgsVehicleMaintenanceCreateDto(1, 1, DateOnly.FromDateTime(DateTime.UtcNow), 60, "ServiceProvider", "InvoiceNumber", 10.5m, null, 60, true, "Description", "Notes value")),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "vehiclemaintenances"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletes()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsVehicleMaintenanceCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsVehicleMaintenanceCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsVehicleMaintenanceCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteFgsVehicleMaintenanceCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsVehicleMaintenanceCommand(new FgsVehicleMaintenanceCreateDto(1, 1, DateOnly.FromDateTime(DateTime.UtcNow), 60, "ServiceProvider", "InvoiceNumber", 10.5m, null, 60, true, "Description", "Notes value")),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteFgsVehicleMaintenanceCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsVehicleMaintenanceWriteService CreateWriteService(FgsInventoryDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

        var auditHelper = new InventoryEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsInventoryDbContext>(context);
        return new FgsVehicleMaintenanceWriteService(context, unitOfWork, auditHelper);
    }

    private static async Task<FgsInventoryDbContext> CreateContextAsync()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

        var options = new DbContextOptionsBuilder<FgsInventoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsInventoryDbContext(options, accessor);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
