using Fgs.Asset.Application.Abstractions.Assets;
using Fgs.Asset.Application.Features.Assets.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.Assets.Queries.ListAssets;

public sealed class ListAssetsQueryHandler(IFgsAssetReadRepository readRepository)
    : IRequestHandler<ListAssetsQuery, ApiResponse<PagedResult<FgsAssetSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsAssetSummaryDto>>> Handle(
        ListAssetsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsAssetSummaryDto>>.Ok(result);
    }
}
