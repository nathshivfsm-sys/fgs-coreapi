using Fgs.Asset.Application.Abstractions.AssetManufacturers;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Asset.Application.Features.AssetManufacturers.Queries.GetFgsAssetManufacturerById;
using Fgs.Asset.Application.Features.AssetManufacturers.Queries.ListAssetManufacturers;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Moq;

namespace Fgs.Asset.Tests.AssetManufacturers;

public sealed class FgsAssetManufacturerQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var readRepository = new Mock<IFgsAssetManufacturerReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(
            new FgsAssetManufacturerDetailDto(1, "CODE01", "Test", null, true));
        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });
        var handler = new GetFgsAssetManufacturerByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsAssetManufacturerByIdQuery(1), CancellationToken.None);
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsAssetManufacturerReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsAssetManufacturerDetailDto?)null);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });
        var handler = new GetFgsAssetManufacturerByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsAssetManufacturerByIdQuery(99), CancellationToken.None);
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsAssetManufacturerReadRepository>();
        readRepository.Setup(r => r.ListAsync(It.IsAny<AssetListQuery>(), It.IsAny<FgsAssetManufacturerListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsAssetManufacturerSummaryDto>([], 1, 25, 0));
        var handler = new ListAssetManufacturersQueryHandler(readRepository.Object);
        var response = await handler.Handle(new ListAssetManufacturersQuery(new AssetListQuery(), new FgsAssetManufacturerListFilters()), CancellationToken.None);
        response.Success.Should().BeTrue();
    }
}
