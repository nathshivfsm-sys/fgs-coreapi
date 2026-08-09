using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Infrastructure.Database;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fgs.Crm.Tests.Infrastructure;

public sealed class CrmTenantQueryFilterTests
{
    [Fact]
    public async Task Customers_WhenUnresolved_ReturnsAllTenants()
    {
        await using var context = await CreateContextAsync();
        context.CrmCustomers.AddRange(
            CreateCustomer(1, 1, "A"),
            CreateCustomer(2, 1, "B"));
        await context.SaveChangesAsync();

        var rows = await context.CrmCustomers.ToListAsync();

        rows.Should().HaveCount(2);
    }

    [Fact]
    public async Task Customers_WhenResolved_DoesNotLeakAcrossTenants()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = 1, CompanyId = 1 }
        };

        await using var context = await CreateContextAsync(accessor);
        context.CrmCustomers.AddRange(
            CreateCustomer(1, 1, "MATCH"),
            CreateCustomer(2, 1, "OTHER_TENANT"),
            CreateCustomer(1, 2, "OTHER_COMPANY"));
        await context.SaveChangesAsync();

        var rows = await context.CrmCustomers.ToListAsync();

        rows.Should().ContainSingle();
        rows[0].CustomerNumber.Should().Be("MATCH");
    }

    private static async Task<FgsCrmDbContext> CreateContextAsync(
        ITenantContextAccessor? accessor = null)
    {
        var options = new DbContextOptionsBuilder<FgsCrmDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsCrmDbContext(
            options,
            accessor ?? new DesignTimeTenantContextAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private static CrmCustomer CreateCustomer(long tenantId, long companyId, string number) =>
        new()
        {
            TenantId = tenantId,
            CompanyId = companyId,
            CustomerNumber = number,
            Name = number,
            DisplayName = number,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
