using Fgs.Contracts.IntegrationEvents;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Inventory.Infrastructure.InventoryStocks;
using Fgs.Messaging.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace Fgs.Inventory.Tests.InventoryStocks;

public sealed class FgsInventoryStockWriteServiceOutboxTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateAsync_EnqueuesInventoryStockChanged()
    {
        await using var context = await CreateContextAsync();
        var itemId = await SeedInventoryItemAsync(context);
        var outbox = CreateOutboxMock();
        var writeService = CreateWriteService(context, outbox.Object);

        var created = await writeService.CreateAsync(
            new FgsInventoryStockCreateDto(itemId, 10m, 1m, 9m, 5m, 5m, null, null),
            CancellationToken.None);

        created.Id.Should().BeGreaterThan(0);
        VerifyEnqueue(outbox, IntegrationEventTypes.InventoryStockChanged, Times.Once());
    }

    [Fact]
    public async Task UpdateAsync_EnqueuesInventoryStockChanged()
    {
        await using var context = await CreateContextAsync();
        var itemId = await SeedInventoryItemAsync(context);
        var outbox = CreateOutboxMock();
        var writeService = CreateWriteService(context, outbox.Object);

        var created = await writeService.CreateAsync(
            new FgsInventoryStockCreateDto(itemId, 10m, 1m, 9m, 5m, 5m, null, null),
            CancellationToken.None);
        outbox.Invocations.Clear();

        await writeService.UpdateAsync(
            created.Id,
            new FgsInventoryStockUpdateDto(itemId, 20m, 2m, 18m, 6m, 6m, null, null),
            CancellationToken.None);

        VerifyEnqueue(outbox, IntegrationEventTypes.InventoryStockChanged, Times.Once());
    }

    private static void VerifyEnqueue(Mock<IOutboxWriter> outbox, string eventType, Times times) =>
        outbox.Verify(
            o => o.EnqueueAsync(
                eventType,
                It.IsAny<string>(),
                It.IsAny<Guid>(),
                TenantId,
                CompanyId,
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                IntegrationEventExchanges.InventoryEvents,
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<long?>(),
                It.IsAny<CancellationToken>()),
            times);

    private static Mock<IOutboxWriter> CreateOutboxMock()
    {
        var outbox = new Mock<IOutboxWriter>();
        outbox
            .Setup(o => o.EnqueueAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Guid>(),
                It.IsAny<long?>(),
                It.IsAny<long?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<long?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        return outbox;
    }

    private static FgsInventoryStockWriteService CreateWriteService(
        FgsInventoryDbContext context,
        IOutboxWriter outboxWriter)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

        var dateTime = new DateTimeProvider();
        var auditHelper = new InventoryEntityAuditHelper(userContext.Object, tenantAccessor, dateTime);
        var unitOfWork = new EfUnitOfWork<FgsInventoryDbContext>(context);

        return new FgsInventoryStockWriteService(
            context,
            unitOfWork,
            auditHelper,
            outboxWriter,
            dateTime);
    }

    private static async Task<long> SeedInventoryItemAsync(FgsInventoryDbContext context)
    {
        var item = new FgsInventoryItem
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            ItemCode = "STOCK-ITEM",
            Name = "Stock Item",
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };
        context.FgsInventoryItems.Add(item);
        await context.SaveChangesAsync();
        return item.Id;
    }

    private static async Task<FgsInventoryDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsInventoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsInventoryDbContext(options, new DesignTimeTenantContextAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
