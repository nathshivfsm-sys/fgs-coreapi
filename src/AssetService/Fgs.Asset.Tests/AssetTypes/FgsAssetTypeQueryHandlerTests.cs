using Fgs.Asset.Application.Abstractions.AssetTypes;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Asset.Application.Features.AssetTypes.Queries.GetFgsAssetTypeById;
using Fgs.Asset.Application.Features.AssetTypes.Queries.ListAssetTypes;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Moq;

namespace Fgs.Asset.Tests.AssetTypes;

public sealed class FgsAssetTypeQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var readRepository = new Mock<IFgsAssetTypeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(
            new FgsAssetTypeDetailDto(1, "CODE01", "Test", null, true));
        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });
        var handler = new GetFgsAssetTypeByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsAssetTypeByIdQuery(1), CancellationToken.None);
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsAssetTypeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsAssetTypeDetailDto?)null);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });
        var handler = new GetFgsAssetTypeByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsAssetTypeByIdQuery(99), CancellationToken.None);
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsAssetTypeReadRepository>();
        readRepository.Setup(r => r.ListAsync(It.IsAny<AssetListQuery>(), It.IsAny<FgsAssetTypeListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsAssetTypeSummaryDto>([], 1, 25, 0));
        var handler = new ListAssetTypesQueryHandler(readRepository.Object);
        var response = await handler.Handle(new ListAssetTypesQuery(new AssetListQuery(), new FgsAssetTypeListFilters()), CancellationToken.None);
        response.Success.Should().BeTrue();
    }
}
