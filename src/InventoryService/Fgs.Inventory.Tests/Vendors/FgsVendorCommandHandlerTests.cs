using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Fgs.Inventory.Application.Features.Vendors.Commands.CreateFgsVendor;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Foundation.Time;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Inventory.Infrastructure.Vendors;
using Microsoft.EntityFrameworkCore;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Fgs.MultiTenancy.Persistence;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Inventory.Tests.Vendors;

public sealed class FgsVendorCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    private static FgsVendorCreateDto SampleCreateDto() =>
        new(
            "VEND01",
            "Acme Supplies",
            "Acme Supplies LLC",
            VendorTypes.Vendor,
            VendorStatuses.Active,
            null,
            null,
            "Jane Doe",
            "Buyer",
            "jane@acme.com",
            null,
            "555-0100",
            null,
            null,
            "https://acme.example",
            "100 Vendor Way",
            null,
            "Austin",
            "TX",
            "78701",
            "US",
            "12-3456789",
            null,
            null,
            "Preferred vendor",
            false);

    [Fact]
    public async Task CreateHandler_CreatesActiveRecord()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsVendorCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsVendorCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsVendorCommand(SampleCreateDto()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.IsActive.Should().BeTrue();
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "vendor"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsVendorWriteService CreateWriteService(FgsInventoryDbContext context)
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
        return new FgsVendorWriteService(context, unitOfWork, auditHelper);
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
