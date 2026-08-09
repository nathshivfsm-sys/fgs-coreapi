using Fgs.Asset.Application.Abstractions.AssetAttributeValues;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeValues.Queries.ListAssetAttributeValues;

public sealed class ListAssetAttributeValuesQueryHandler(IFgsAssetAttributeValueReadRepository readRepository)
    : IRequestHandler<ListAssetAttributeValuesQuery, ApiResponse<PagedResult<FgsAssetAttributeValueSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsAssetAttributeValueSummaryDto>>> Handle(
        ListAssetAttributeValuesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsAssetAttributeValueSummaryDto>>.Ok(result);
    }
}
