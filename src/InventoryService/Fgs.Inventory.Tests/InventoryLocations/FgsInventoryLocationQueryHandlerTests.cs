using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using Fgs.Inventory.Application.Features.InventoryLocations.Queries.GetFgsInventoryLocationById;
using Fgs.Inventory.Application.Features.InventoryLocations.Queries.ListInventoryLocations;
using Fgs.Inventory.Domain.Entities;
using Moq;

namespace Fgs.Inventory.Tests.InventoryLocations;

public sealed class FgsInventoryLocationQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsInventoryLocationDetailDto(
            1, "WH01", "Main Warehouse", InventoryLocationTypes.Warehouse, null, "Primary storage",
            "123 Main St", null, "Austin", "TX", "78701", "US", "Contact", "555-0100", "wh@example.com", false, true);

        var readRepository = new Mock<IFgsInventoryLocationReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsInventoryLocationByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsInventoryLocationByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsInventoryLocationReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsInventoryLocationDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsInventoryLocationByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsInventoryLocationByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsInventoryLocationReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<InventoryListQuery>(), It.IsAny<FgsInventoryLocationListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsInventoryLocationSummaryDto>([], 1, 25, 0));

        var handler = new ListInventoryLocationsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListInventoryLocationsQuery(new InventoryListQuery(), new FgsInventoryLocationListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
