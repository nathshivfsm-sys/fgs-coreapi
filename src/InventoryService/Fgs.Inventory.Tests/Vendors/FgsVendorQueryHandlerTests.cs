using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using Fgs.Inventory.Application.Features.Vendors.Queries.GetFgsVendorById;
using Fgs.Inventory.Application.Features.Vendors.Queries.ListVendors;
using Fgs.Inventory.Domain.Entities;
using Moq;

namespace Fgs.Inventory.Tests.Vendors;

public sealed class FgsVendorQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsVendorDetailDto(
            1, "VEND01", "Acme Supplies", "Acme Supplies LLC", VendorTypes.Vendor, VendorStatuses.Active,
            null, null, "Jane Doe", "Buyer", "jane@acme.com", null, "555-0100", null, null,
            "https://acme.example", "100 Vendor Way", null, "Austin", "TX", "78701", "US",
            "12-3456789", null, null, "Preferred vendor", false, true);

        var readRepository = new Mock<IFgsVendorReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsVendorByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsVendorByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsVendorReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsVendorDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsVendorByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsVendorByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsVendorReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<InventoryListQuery>(), It.IsAny<FgsVendorListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsVendorSummaryDto>([], 1, 25, 0));

        var handler = new ListVendorsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListVendorsQuery(new InventoryListQuery(), new FgsVendorListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
