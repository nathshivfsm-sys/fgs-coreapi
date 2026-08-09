using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fgs.Inventory.Tests.Infrastructure;

public sealed class InventoryTenantQueryFilterTests
{
    [Fact]
    public async Task Locations_WhenUnresolved_ReturnsAllTenants()
    {
        await using var context = await CreateContextAsync();
        context.FgsInventoryLocations.AddRange(
            CreateLocation(1, 1, "A"),
            CreateLocation(2, 1, "B"));
        await context.SaveChangesAsync();

        var rows = await context.FgsInventoryLocations.ToListAsync();

        rows.Should().HaveCount(2);
    }

    [Fact]
    public async Task Locations_WhenResolved_DoesNotLeakAcrossTenants()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = 1, CompanyId = 1 }
        };

        await using var context = await CreateContextAsync(accessor);
        context.FgsInventoryLocations.AddRange(
            CreateLocation(1, 1, "MATCH"),
            CreateLocation(2, 1, "OTHER_TENANT"),
            CreateLocation(1, 2, "OTHER_COMPANY"));
        await context.SaveChangesAsync();

        var rows = await context.FgsInventoryLocations.ToListAsync();

        rows.Should().ContainSingle();
        rows[0].InventoryLocationCode.Should().Be("MATCH");
    }

    private static async Task<FgsInventoryDbContext> CreateContextAsync(
        ITenantContextAccessor? accessor = null)
    {
        var options = new DbContextOptionsBuilder<FgsInventoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsInventoryDbContext(
            options,
            accessor ?? new DesignTimeTenantContextAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private static FgsInventoryLocation CreateLocation(long tenantId, long companyId, string code) =>
        new()
        {
            TenantId = tenantId,
            CompanyId = companyId,
            InventoryLocationCode = code,
            Name = code,
            InventoryLocationType = InventoryLocationTypes.Warehouse,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
