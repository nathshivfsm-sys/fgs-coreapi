using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.CreateFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.DeleteFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.CreateFgsTruckStockTemplateItem;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.DeleteFgsTruckStockTemplateItem;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Common.Time;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Inventory.Infrastructure.TruckStockTemplateItems;
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

    private static FgsTruckStockTemplateCreateDto SampleCreateDto() =>
        new("TRUCK-STD", "Standard Truck", "Default truck stock");

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
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "truck-stock-template"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteHandler_SoftDeletes()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateTemplateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var createHandler = new CreateFgsTruckStockTemplateCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsTruckStockTemplateCommandHandler>.Instance);
        var deleteHandler = new DeleteFgsTruckStockTemplateCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteFgsTruckStockTemplateCommandHandler>.Instance);

        var created = await createHandler.Handle(
            new CreateFgsTruckStockTemplateCommand(SampleCreateDto()),
            CancellationToken.None);
        created.Success.Should().BeTrue();

        var response = await deleteHandler.Handle(
            new DeleteFgsTruckStockTemplateCommand(created.Data!.Id),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task CreateItemHandler_CreatesItem_And_DeleteHandler_HardDeletes()
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
        context.FgsInventoryItems.Add(inventoryItem);
        await context.SaveChangesAsync();

        var templateWrite = CreateTemplateWriteService(context);
        var itemWrite = CreateItemWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();

        var template = await new CreateFgsTruckStockTemplateCommandHandler(
            templateWrite,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsTruckStockTemplateCommandHandler>.Instance).Handle(
            new CreateFgsTruckStockTemplateCommand(SampleCreateDto()),
            CancellationToken.None);

        var createItemHandler = new CreateFgsTruckStockTemplateItemCommandHandler(
            itemWrite,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsTruckStockTemplateItemCommandHandler>.Instance);

        var createdItem = await createItemHandler.Handle(
            new CreateFgsTruckStockTemplateItemCommand(
                template.Data!.Id,
                new FgsTruckStockTemplateItemCreateDto(inventoryItem.Id, 5m, 1m, 1)),
            CancellationToken.None);

        createdItem.Success.Should().BeTrue();
        createdItem.StatusCode.Should().Be(201);

        var deleteItemHandler = new DeleteFgsTruckStockTemplateItemCommandHandler(
            itemWrite,
            cache.Object,
            tenantAccessor,
            NullLogger<DeleteFgsTruckStockTemplateItemCommandHandler>.Instance);

        var deleted = await deleteItemHandler.Handle(
            new DeleteFgsTruckStockTemplateItemCommand(template.Data.Id, createdItem.Data!.Id),
            CancellationToken.None);

        deleted.Success.Should().BeTrue();
        (await context.FgsTruckStockTemplateItems.CountAsync()).Should().Be(0);
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

    private static FgsTruckStockTemplateItemWriteService CreateItemWriteService(FgsInventoryDbContext context)
    {
        var (auditHelper, unitOfWork) = CreateAuditAndUow(context);
        return new FgsTruckStockTemplateItemWriteService(context, unitOfWork, auditHelper);
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
