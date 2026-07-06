using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetModels.Queries.ListAssetModels;

public sealed class ListAssetModelsQueryHandler(IFgsAssetModelReadRepository readRepository)
    : IRequestHandler<ListAssetModelsQuery, ApiResponse<PagedResult<FgsAssetModelSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsAssetModelSummaryDto>>> Handle(
        ListAssetModelsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsAssetModelSummaryDto>>.Ok(result);
    }
}
