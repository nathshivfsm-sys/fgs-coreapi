using Fgs.Contracts.Api;
using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Common.BillingCrud;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Billing.Application.Features.Invoices.Queries.GetFgsInvoiceById;
using Fgs.Billing.Application.Features.Invoices.Queries.ListFgsInvoices;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Moq;

namespace Fgs.Billing.Tests.Invoices;

public sealed class FgsInvoiceQueryHandlerTests
{
    private static FgsInvoiceDetailDto SampleDetail() =>
        new(
            1,
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
            false,
            null,
            100m,
            0m,
            100m,
            8m,
            108m,
            0m,
            108m,
            false,
            null,
            null,
            false,
            null,
            null,
            null,
            null,
            null,
            Array.Empty<FgsInvoiceLineDto>());

    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var readRepository = new Mock<IFgsInvoiceReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(SampleDetail());

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsInvoiceByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsInvoiceByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsInvoiceReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsInvoiceDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsInvoiceByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsInvoiceByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsInvoiceReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(
                It.IsAny<BillingListQuery>(),
                It.IsAny<FgsInvoiceListFilters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsInvoiceSummaryDto>([], 1, 25, 0));

        var handler = new ListFgsInvoicesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListFgsInvoicesQuery(new BillingListQuery(), new FgsInvoiceListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
