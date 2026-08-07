using Fgs.Billing.Domain.Entities;
using Fgs.Billing.Infrastructure.Database;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fgs.Billing.Tests.Infrastructure;

public sealed class BillingTenantQueryFilterTests
{
    [Fact]
    public async Task Invoices_WhenUnresolved_ReturnsAllTenants()
    {
        await using var context = await CreateContextAsync();
        context.FgsInvoices.AddRange(
            CreateInvoice(1, 1, "INV-A"),
            CreateInvoice(2, 1, "INV-B"));
        await context.SaveChangesAsync();

        var rows = await context.FgsInvoices.ToListAsync();

        rows.Should().HaveCount(2);
    }

    [Fact]
    public async Task Invoices_WhenResolved_DoesNotLeakAcrossTenants()
    {
        var accessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = 1, CompanyId = 1 }
        };

        await using var context = await CreateContextAsync(accessor);
        context.FgsInvoices.AddRange(
            CreateInvoice(1, 1, "MATCH"),
            CreateInvoice(2, 1, "OTHER_TENANT"),
            CreateInvoice(1, 2, "OTHER_COMPANY"));
        await context.SaveChangesAsync();

        var rows = await context.FgsInvoices.ToListAsync();

        rows.Should().ContainSingle();
        rows[0].InvoiceNumber.Should().Be("MATCH");
    }

    private static async Task<FgsBillingDbContext> CreateContextAsync(
        ITenantContextAccessor? accessor = null)
    {
        var options = new DbContextOptionsBuilder<FgsBillingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsBillingDbContext(
            options,
            accessor ?? new DesignTimeTenantContextAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private static FgsInvoice CreateInvoice(long tenantId, long companyId, string number) =>
        new()
        {
            TenantId = tenantId,
            CompanyId = companyId,
            InvoiceNumber = number,
            InvoiceTypeId = 1,
            CustomerId = 100,
            ServiceLocationId = 200,
            InvoiceDate = new DateOnly(2026, 8, 1),
            AccountingDate = new DateOnly(2026, 8, 1),
            InvoiceSubtotal = 100m,
            TotalDiscount = 0m,
            TaxableAmount = 100m,
            TotalTax = 0m,
            InvoiceTotal = 100m,
            AppliedAmount = 0m,
            BalanceDue = 100m,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = 1
        };

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
