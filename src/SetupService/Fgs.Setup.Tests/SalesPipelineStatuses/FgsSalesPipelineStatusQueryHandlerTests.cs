using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.GetFgsSalesPipelineStatusById;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Queries.ListSalesPipelineStatuses;
using Moq;

namespace Fgs.Setup.Tests.SalesPipelineStatuses;

public sealed class FgsSalesPipelineStatusQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSalesPipelineStatusDetailDto(1, 10, 20, "TEST", "StatusName", "Description", 5, false, true, false, false, true, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSalesPipelineStatusReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20, IsResolved = true });

        var handler = new GetFgsSalesPipelineStatusByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsSalesPipelineStatusByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSalesPipelineStatusReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSalesPipelineStatusDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20, IsResolved = true });

        var handler = new GetFgsSalesPipelineStatusByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsSalesPipelineStatusByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSalesPipelineStatusReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSalesPipelineStatusListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSalesPipelineStatusSummaryDto>([], 1, 25, 0));

        var handler = new ListSalesPipelineStatusesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSalesPipelineStatusesQuery(new SetupListQuery(), new FgsSalesPipelineStatusListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
