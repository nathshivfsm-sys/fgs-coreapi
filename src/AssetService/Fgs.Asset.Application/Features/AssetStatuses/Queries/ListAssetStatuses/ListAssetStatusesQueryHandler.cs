using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetStatuses.Queries.ListAssetStatuses;

public sealed class ListAssetStatusesQueryHandler(IFgsAssetStatusReadRepository readRepository)
    : IRequestHandler<ListAssetStatusesQuery, ApiResponse<PagedResult<FgsAssetStatusSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsAssetStatusSummaryDto>>> Handle(
        ListAssetStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsAssetStatusSummaryDto>>.Ok(result);
    }
}
