using Fgs.Asset.Application.Abstractions.AssetTypes;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetTypes.Queries.ListAssetTypes;

public sealed class ListAssetTypesQueryHandler(IFgsAssetTypeReadRepository readRepository)
    : IRequestHandler<ListAssetTypesQuery, ApiResponse<PagedResult<FgsAssetTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsAssetTypeSummaryDto>>> Handle(
        ListAssetTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsAssetTypeSummaryDto>>.Ok(result);
    }
}
