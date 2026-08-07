using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Application.Features.Customers.Queries.GetCrmCustomerById;
using Fgs.Crm.Application.Features.Customers.Queries.ListCrmCustomers;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Moq;

namespace Fgs.Crm.Tests.Customers;

public sealed class CrmCustomerQueryHandlerTests
{
    private static CrmCustomerDetailDto SampleDetail() =>
        new(
            1,
            "CUST01",
            "Acme Corporation",
            "Acme Corp",
            "100 Main St",
            null,
            null,
            null,
            "Austin",
            "TX",
            null,
            "US",
            "78701",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            false,
            false,
            null,
            "ACCT-100",
            null,
            null,
            true);

    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var readRepository = new Mock<ICrmCustomerReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(SampleDetail());

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetCrmCustomerByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetCrmCustomerByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<ICrmCustomerReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CrmCustomerDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetCrmCustomerByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetCrmCustomerByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<ICrmCustomerReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(
                It.IsAny<CrmListQuery>(),
                It.IsAny<CrmCustomerListFilters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<CrmCustomerSummaryDto>([], 1, 25, 0));

        var handler = new ListCrmCustomersQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListCrmCustomersQuery(new CrmListQuery(), new CrmCustomerListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
