using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Asset.Application.Features.AssetAttributes.Queries.ListAssetAttributes;
using Fgs.Foundation.Paging;
using Moq;
namespace Fgs.Asset.Tests.AssetAttributes;
public sealed class FgsAssetAttributeQueryHandlerTests
{
  [Fact] public async Task List_ReturnsPagedResult() { var repo = new Mock<IFgsAssetAttributeReadRepository>(); repo.Setup(r => r.ListAsync(It.IsAny<AssetListQuery>(), It.IsAny<FgsAssetAttributeListFilters>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PagedResult<FgsAssetAttributeSummaryDto>([], 1, 25, 0)); var h = new ListAssetAttributesQueryHandler(repo.Object); var res = await h.Handle(new ListAssetAttributesQuery(new AssetListQuery(), new FgsAssetAttributeListFilters()), CancellationToken.None); res.Success.Should().BeTrue(); }
}
