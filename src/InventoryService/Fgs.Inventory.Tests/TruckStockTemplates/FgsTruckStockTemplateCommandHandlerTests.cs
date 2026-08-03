using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.CreateFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.UpdateFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Common.Time;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Inventory.Infrastructure.TruckStockTemplates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Inventory.Tests.TruckStockTemplates;

public sealed class FgsTruckStockTemplateCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    private static FgsTruckStockTemplateCreateDto SampleCreateDto(
        IReadOnlyList<FgsTruckStockTemplateItemDto>? items = null) =>
        new("TRUCK-STD", "Standard Truck", "Default truck stock", items);

    [Fact]
    public async Task CreateHandler_CreatesActiveRecord()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateTemplateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsTruckStockTemplateCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsTruckStockTemplateCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsTruckStockTemplateCommand(SampleCreateDto()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        response.Data.Items.Should().BeEmpty();
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "truckstocktemplate"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateHandler_WithItems_CreatesItems_And_Update_RemovesOmitted()
    {
        await using var context = await CreateContextAsync();
        var inventoryItem = new FgsInventoryItem
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            ItemCode = "ITEM01",
            Name = "Filter",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        var inventoryItem2 = new FgsInventoryItem
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            ItemCode = "ITEM02",
            Name = "Belt",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsInventoryItems.AddRange(inventoryItem, inventoryItem2);
        await context.SaveChangesAsync();

        var writeService = CreateTemplateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();

        var created = await new CreateFgsTruckStockTemplateCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsTruckStockTemplateCommandHandler>.Instance).Handle(
            new CreateFgsTruckStockTemplateCommand(
                SampleCreateDto(
                [
                    new FgsTruckStockTemplateItemDto(null, inventoryItem.Id, 5m, 1m, 1),
                    new FgsTruckStockTemplateItemDto(null, inventoryItem2.Id, 2m, 1m, 2)
                ])),
            CancellationToken.None);

        created.Success.Should().BeTrue();
        created.Data!.Items.Should().HaveCount(2);
        (await context.FgsTruckStockTemplateItems.CountAsync()).Should().Be(2);

        var keepId = created.Data.Items[0].Id;
        var updated = await new UpdateFgsTruckStockTemplateCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<UpdateFgsTruckStockTemplateCommandHandler>.Instance).Handle(
            new UpdateFgsTruckStockTemplateCommand(
                created.Data.Id,
                new FgsTruckStockTemplateUpdateDto(
                    "TRUCK-STD",
                    "Standard Truck",
                    "Default truck stock",
                    [new FgsTruckStockTemplateItemDto(keepId, inventoryItem.Id, 10m, 2m, 1)])),
            CancellationToken.None);

        updated.Success.Should().BeTrue();
        updated.Data!.Items.Should().HaveCount(1);
        updated.Data.Items[0].TargetQuantity.Should().Be(10m);
        (await context.FgsTruckStockTemplateItems.CountAsync()).Should().Be(1);
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsTruckStockTemplateWriteService CreateTemplateWriteService(FgsInventoryDbContext context)
    {
        var (auditHelper, unitOfWork) = CreateAuditAndUow(context);
        return new FgsTruckStockTemplateWriteService(context, unitOfWork, auditHelper);
    }

    private static (InventoryEntityAuditHelper AuditHelper, EfUnitOfWork<FgsInventoryDbContext> UnitOfWork) CreateAuditAndUow(
        FgsInventoryDbContext context)
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
        return (auditHelper, unitOfWork);
    }

    private static async Task<FgsInventoryDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsInventoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsInventoryDbContext(options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
