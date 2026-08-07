using Fgs.Billing.Application.Features.Invoices.Commands.CreateFgsInvoice;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Billing.Infrastructure.Common;
using Fgs.Billing.Infrastructure.Common.Time;
using Fgs.Billing.Infrastructure.Database;
using Fgs.Billing.Infrastructure.Persistence.Invoices;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Billing.Tests.Invoices;

public sealed class FgsInvoiceCommandHandlerTests
{
    private const long TenantId = 10;
    private const long CompanyId = 20;

    private static FgsInvoiceCreateDto SampleCreateDto() =>
        new(
            "INV-001",
            1,
            100,
            200,
            null,
            null,
            null,
            null,
            null,
            false,
            false,
            null,
            null,
            null,
            null,
            null,
            new DateOnly(2026, 8, 1),
            new DateOnly(2026, 8, 1),
            new DateOnly(2026, 8, 31),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            100m,
            0m,
            100m,
            8m,
            108m,
            108m);

    [Fact]
    public async Task CreateHandler_CreatesInvoiceRecord()
    {
        await using var context = await CreateContextAsync();
        var writeService = CreateWriteService(context);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = CreateTenantContextAccessor();
        var handler = new CreateFgsInvoiceCommandHandler(
            writeService,
            cache.Object,
            tenantAccessor,
            NullLogger<CreateFgsInvoiceCommandHandler>.Instance);

        var response = await handler.Handle(
            new CreateFgsInvoiceCommand(SampleCreateDto()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(201);
        response.Data!.InvoiceNumber.Should().Be("INV-001");
        response.Data.CustomerId.Should().Be(100);
        cache.Verify(
            c => c.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(TenantId, CompanyId, "invoice"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static ITenantContextAccessor CreateTenantContextAccessor() =>
        new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

    private static FgsInvoiceWriteService CreateWriteService(FgsBillingDbContext context)
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(x => x.TenantId).Returns(TenantId);
        userContext.SetupGet(x => x.CompanyId).Returns(CompanyId);
        userContext.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var tenantAccessor = new TestTenantContextAccessor
        {
            Current = new TenantContext { TenantId = TenantId, CompanyId = CompanyId }
        };

        var auditHelper = new BillingEntityAuditHelper(
            userContext.Object,
            tenantAccessor,
            new DateTimeProvider());
        var unitOfWork = new EfUnitOfWork<FgsBillingDbContext>(context);
        return new FgsInvoiceWriteService(context, unitOfWork, auditHelper);
    }

    private static async Task<FgsBillingDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsBillingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsBillingDbContext(options, new DesignTimeTenantContextAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    private sealed class TestTenantContextAccessor : ITenantContextAccessor
    {
        public ITenantContext? Current { get; set; }
    }
}
