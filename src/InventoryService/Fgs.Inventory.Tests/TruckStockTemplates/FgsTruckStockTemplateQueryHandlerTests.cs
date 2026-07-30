using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.GetFgsTruckStockTemplateById;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.ListTruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Queries.GetFgsTruckStockTemplateItemById;
using Moq;

namespace Fgs.Inventory.Tests.TruckStockTemplates;

public sealed class FgsTruckStockTemplateQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsTruckStockTemplateDetailDto(1, "TRUCK-STD", "Standard Truck", null, true);

        var readRepository = new Mock<IFgsTruckStockTemplateReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsTruckStockTemplateByIdQueryHandler(
            readRepository.Object,
            cache.Object,
            tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsTruckStockTemplateByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsTruckStockTemplateReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsTruckStockTemplateDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsTruckStockTemplateByIdQueryHandler(
            readRepository.Object,
            cache.Object,
            tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsTruckStockTemplateByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsTruckStockTemplateReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(
                It.IsAny<InventoryListQuery>(),
                It.IsAny<FgsTruckStockTemplateListFilters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsTruckStockTemplateSummaryDto>([], 1, 25, 0));

        var handler = new ListTruckStockTemplatesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListTruckStockTemplatesQuery(new InventoryListQuery(), new FgsTruckStockTemplateListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }

    [Fact]
    public async Task GetItemById_WhenTemplateMismatch_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsTruckStockTemplateItemReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(1, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsTruckStockTemplateItemDetailDto?)null);

        var handler = new GetFgsTruckStockTemplateItemByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new GetFgsTruckStockTemplateItemByIdQuery(1, 5),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }
}
