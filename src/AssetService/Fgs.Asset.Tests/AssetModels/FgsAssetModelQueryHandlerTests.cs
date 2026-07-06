using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Asset.Application.Features.AssetModels.Queries.GetFgsAssetModelById;
using Fgs.Asset.Application.Features.AssetModels.Queries.ListAssetModels;
using Moq;

namespace Fgs.Asset.Tests.AssetModels;

public sealed class FgsAssetModelQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsAssetModelDetailDto(1, 1, 1, "58MCA", "Carrier Infinity Model", true);
        var readRepository = new Mock<IFgsAssetModelReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);
        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsAssetModelByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsAssetModelByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsAssetModelReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<AssetListQuery>(), It.IsAny<FgsAssetModelListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsAssetModelSummaryDto>([], 1, 25, 0));

        var handler = new ListAssetModelsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListAssetModelsQuery(new AssetListQuery(), new FgsAssetModelListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
