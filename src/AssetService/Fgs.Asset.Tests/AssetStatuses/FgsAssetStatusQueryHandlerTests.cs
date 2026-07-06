using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Asset.Application.Features.AssetStatuses.Queries.GetFgsAssetStatusById;
using Fgs.Asset.Application.Features.AssetStatuses.Queries.ListAssetStatuses;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Moq;

namespace Fgs.Asset.Tests.AssetStatuses;

public sealed class FgsAssetStatusQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var readRepository = new Mock<IFgsAssetStatusReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(
            new FgsAssetStatusDetailDto(1, "CODE01", "Test", null, true));
        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });
        var handler = new GetFgsAssetStatusByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsAssetStatusByIdQuery(1), CancellationToken.None);
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsAssetStatusReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsAssetStatusDetailDto?)null);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });
        var handler = new GetFgsAssetStatusByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsAssetStatusByIdQuery(99), CancellationToken.None);
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsAssetStatusReadRepository>();
        readRepository.Setup(r => r.ListAsync(It.IsAny<AssetListQuery>(), It.IsAny<FgsAssetStatusListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsAssetStatusSummaryDto>([], 1, 25, 0));
        var handler = new ListAssetStatusesQueryHandler(readRepository.Object);
        var response = await handler.Handle(new ListAssetStatusesQuery(new AssetListQuery(), new FgsAssetStatusListFilters()), CancellationToken.None);
        response.Success.Should().BeTrue();
    }
}
