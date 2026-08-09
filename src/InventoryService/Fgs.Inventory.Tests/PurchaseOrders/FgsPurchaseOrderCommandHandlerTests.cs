using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Messaging.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Inventory.Application.Features.PurchaseOrders.Commands.CreateFgsPurchaseOrder;
using Fgs.Inventory.Application.Features.PurchaseOrders.Commands.UpdateFgsPurchaseOrder;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Inventory.Infrastructure.PurchaseOrders;
using Microsoft.EntityFrameworkCore;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Inventory.Tests.PurchaseOrders;

public sealed class FgsPurchaseOrderCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    private static FgsPurchaseOrderCreateDto SampleCreateDto(
        IReadOnlyList<FgsPurchaseOrderLineDto>? details = null) =>
        new(
            "PO-1001",
            VendorId: 1,
            PurchaseOrderStatuses.Open,
            PurchaseOrderDate: DateTimeOffset.UtcNow,
            ExpectedDeliveryDate: null,
            RequestedByEmployeeId: null,
            RequestedByName: null,
            BuyerEmployeeId: null,
            ShipToInventoryLocationId: null,
            ShipToServiceLocationId: null,
            ShipToName: null,
            ShipToAddress1: null,
            ShipToAddress2: null,
            ShipToCity: null,
            ShipToStateProvince: null,
            ShipToPostalCode: null,
            ShipToCountry: null,
            VendorReferenceNumber: null,
            VendorContactName: null,
            VendorEmail: null,
            VendorPhoneNumber: null,
            Subtotal: 100m,
            DiscountAmount: 0m,
            TaxableAmount: 100m,
            PurchaseTaxJson: null,
            FreightAmount: 0m,
            OtherCharges: 0m,
            TotalAmount: 100m,
            VendorNotes: null,
            InternalNotes: null,
            details);

    [Fact]
    public async Task CreateHandler_CreatesRecordWithoutDetails()
    {
        await using var context = await CreateContextAsync();
        await SeedVendorAsync(context);
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsPurchaseOrderCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsPurchaseOrderCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsPurchaseOrderCommand(SampleCreateDto()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.Details.Should().BeEmpty();
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "purchaseorder"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateHandler_WithDetails_CreatesLines_And_Update_RemovesOmitted()
    {
        await using var context = await CreateContextAsync();
        await SeedVendorAsync(context);
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

        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();

        var created = await new CreateFgsPurchaseOrderCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsPurchaseOrderCommandHandler>.Instance).Handle(
            new CreateFgsPurchaseOrderCommand(
                SampleCreateDto(
                [
                    new FgsPurchaseOrderLineDto(null, 1, inventoryItem.Id, null, "Filter", "EA", 5m, 0m, 10m, 0m, true, 50m, null, null),
                    new FgsPurchaseOrderLineDto(null, 2, inventoryItem2.Id, null, "Belt", "EA", 2m, 0m, 20m, 0m, true, 40m, null, null)
                ])),
            CancellationToken.None);

        created.Success.Should().BeTrue();
        created.Data!.Details.Should().HaveCount(2);
        (await context.FgsPurchaseOrderDetails.CountAsync()).Should().Be(2);

        var keepId = created.Data.Details[0].Id;
        var updated = await new UpdateFgsPurchaseOrderCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<UpdateFgsPurchaseOrderCommandHandler>.Instance).Handle(
            new UpdateFgsPurchaseOrderCommand(
                created.Data.Id,
                new FgsPurchaseOrderUpdateDto(
                    "PO-1001",
                    VendorId: 1,
                    PurchaseOrderStatuses.Open,
                    PurchaseOrderDate: DateTimeOffset.UtcNow,
                    ExpectedDeliveryDate: null,
                    RequestedByEmployeeId: null,
                    RequestedByName: null,
                    BuyerEmployeeId: null,
                    ShipToInventoryLocationId: null,
                    ShipToServiceLocationId: null,
                    ShipToName: null,
                    ShipToAddress1: null,
                    ShipToAddress2: null,
                    ShipToCity: null,
                    ShipToStateProvince: null,
                    ShipToPostalCode: null,
                    ShipToCountry: null,
                    VendorReferenceNumber: null,
                    VendorContactName: null,
                    VendorEmail: null,
                    VendorPhoneNumber: null,
                    Subtotal: 100m,
                    DiscountAmount: 0m,
                    TaxableAmount: 100m,
                    PurchaseTaxJson: null,
                    FreightAmount: 0m,
                    OtherCharges: 0m,
                    TotalAmount: 100m,
                    VendorNotes: null,
                    InternalNotes: null,
                    [new FgsPurchaseOrderLineDto(keepId, 1, inventoryItem.Id, null, "Filter", "EA", 10m, 0m, 10m, 0m, true, 100m, null, null)])),
            CancellationToken.None);

        updated.Success.Should().BeTrue();
        updated.Data!.Details.Should().HaveCount(1);
        updated.Data.Details[0].OrderedQuantity.Should().Be(10m);
        (await context.FgsPurchaseOrderDetails.CountAsync()).Should().Be(1);
    }

    private static async Task SeedVendorAsync(FgsInventoryDbContext context)
    {
        context.FgsVendors.Add(new FgsVendor
        {
            TenantId = TenantId,
            CompanyId = CompanyId,
            VendorCode = "VEND01",
            Name = "Acme Supply",
            VendorType = VendorTypes.Vendor,
            VendorStatus = VendorStatuses.Active,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync();
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsPurchaseOrderWriteService CreateWriteService(FgsInventoryDbContext context)
    {
        var (auditHelper, unitOfWork) = CreateAuditAndUow(context);
        var outboxWriter = new Mock<IOutboxWriter>();
        outboxWriter
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

        return new FgsPurchaseOrderWriteService(
            context,
            unitOfWork,
            auditHelper,
            outboxWriter.Object,
            new DateTimeProvider());
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

        var context = new FgsInventoryDbContext(options, new DesignTimeTenantContextAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
