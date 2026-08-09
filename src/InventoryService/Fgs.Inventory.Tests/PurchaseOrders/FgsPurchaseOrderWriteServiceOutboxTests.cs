using Fgs.Contracts.IntegrationEvents;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Inventory.Infrastructure.PurchaseOrders;
using Fgs.Messaging.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace Fgs.Inventory.Tests.PurchaseOrders;

public sealed class FgsPurchaseOrderWriteServiceOutboxTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    [Fact]
    public async Task CreateAsync_EnqueuesPurchaseOrderStatusChanged()
    {
        await using var context = await CreateContextAsync();
        await SeedVendorAsync(context);
        var outbox = CreateOutboxMock();
        var writeService = CreateWriteService(context, outbox.Object);

        await writeService.CreateAsync(SampleCreateDto(PurchaseOrderStatuses.Open), CancellationToken.None);

        VerifyEnqueue(outbox, IntegrationEventTypes.PurchaseOrderStatusChanged, Times.Once());
    }

    [Fact]
    public async Task UpdateAsync_WhenStatusUnchanged_DoesNotEnqueue()
    {
        await using var context = await CreateContextAsync();
        await SeedVendorAsync(context);
        var outbox = CreateOutboxMock();
        var writeService = CreateWriteService(context, outbox.Object);

        var created = await writeService.CreateAsync(
            SampleCreateDto(PurchaseOrderStatuses.Open),
            CancellationToken.None);
        outbox.Invocations.Clear();

        await writeService.UpdateAsync(
            created.Id,
            SampleUpdateDto(PurchaseOrderStatuses.Open, vendorNotes: "note"),
            CancellationToken.None);

        VerifyEnqueue(outbox, IntegrationEventTypes.PurchaseOrderStatusChanged, Times.Never());
    }

    [Fact]
    public async Task UpdateAsync_WhenStatusChanges_EnqueuesPurchaseOrderStatusChanged()
    {
        await using var context = await CreateContextAsync();
        await SeedVendorAsync(context);
        var outbox = CreateOutboxMock();
        var writeService = CreateWriteService(context, outbox.Object);

        var created = await writeService.CreateAsync(
            SampleCreateDto(PurchaseOrderStatuses.Open),
            CancellationToken.None);
        outbox.Invocations.Clear();

        await writeService.UpdateAsync(
            created.Id,
            SampleUpdateDto(PurchaseOrderStatuses.Cancelled),
            CancellationToken.None);

        VerifyEnqueue(outbox, IntegrationEventTypes.PurchaseOrderStatusChanged, Times.Once());
    }

    [Fact]
    public async Task PatchAsync_WhenStatusUnchanged_DoesNotEnqueue()
    {
        await using var context = await CreateContextAsync();
        await SeedVendorAsync(context);
        var outbox = CreateOutboxMock();
        var writeService = CreateWriteService(context, outbox.Object);

        var created = await writeService.CreateAsync(
            SampleCreateDto(PurchaseOrderStatuses.Open),
            CancellationToken.None);
        outbox.Invocations.Clear();

        await writeService.PatchAsync(
            created.Id,
            new FgsPurchaseOrderPatchDto(
                PurchaseOrderNumber: null,
                VendorId: null,
                PurchaseOrderStatus: PurchaseOrderStatuses.Open,
                PurchaseOrderDate: null,
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
                Subtotal: null,
                DiscountAmount: null,
                TaxableAmount: null,
                PurchaseTaxJson: null,
                FreightAmount: null,
                OtherCharges: null,
                TotalAmount: null,
                VendorNotes: "patched",
                InternalNotes: null,
                Details: null),
            CancellationToken.None);

        VerifyEnqueue(outbox, IntegrationEventTypes.PurchaseOrderStatusChanged, Times.Never());
    }

    [Fact]
    public async Task PatchAsync_WhenStatusChanges_EnqueuesPurchaseOrderStatusChanged()
    {
        await using var context = await CreateContextAsync();
        await SeedVendorAsync(context);
        var outbox = CreateOutboxMock();
        var writeService = CreateWriteService(context, outbox.Object);

        var created = await writeService.CreateAsync(
            SampleCreateDto(PurchaseOrderStatuses.Open),
            CancellationToken.None);
        outbox.Invocations.Clear();

        await writeService.PatchAsync(
            created.Id,
            new FgsPurchaseOrderPatchDto(
                PurchaseOrderNumber: null,
                VendorId: null,
                PurchaseOrderStatus: PurchaseOrderStatuses.Received,
                PurchaseOrderDate: null,
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
                Subtotal: null,
                DiscountAmount: null,
                TaxableAmount: null,
                PurchaseTaxJson: null,
                FreightAmount: null,
                OtherCharges: null,
                TotalAmount: null,
                VendorNotes: null,
                InternalNotes: null,
                Details: null),
            CancellationToken.None);

        VerifyEnqueue(outbox, IntegrationEventTypes.PurchaseOrderStatusChanged, Times.Once());
    }

    private static FgsPurchaseOrderCreateDto SampleCreateDto(string status) =>
        new(
            "PO-OUTBOX-1",
            VendorId: 1,
            status,
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
            Details: null);

    private static FgsPurchaseOrderUpdateDto SampleUpdateDto(
        string status,
        string? vendorNotes = null) =>
        new(
            "PO-OUTBOX-1",
            VendorId: 1,
            status,
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
            VendorNotes: vendorNotes,
            InternalNotes: null,
            Details: null);

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

    private static FgsPurchaseOrderWriteService CreateWriteService(
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

        return new FgsPurchaseOrderWriteService(
            context,
            unitOfWork,
            auditHelper,
            outboxWriter,
            dateTime);
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
