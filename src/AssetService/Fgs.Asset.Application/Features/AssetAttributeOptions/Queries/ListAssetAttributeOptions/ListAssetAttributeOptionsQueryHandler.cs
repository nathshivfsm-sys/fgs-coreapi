using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Queries.ListAssetAttributeOptions;

public sealed class ListAssetAttributeOptionsQueryHandler(IFgsAssetAttributeOptionReadRepository readRepository)
    : IRequestHandler<ListAssetAttributeOptionsQuery, ApiResponse<PagedResult<FgsAssetAttributeOptionSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsAssetAttributeOptionSummaryDto>>> Handle(
        ListAssetAttributeOptionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsAssetAttributeOptionSummaryDto>>.Ok(result);
    }
}
