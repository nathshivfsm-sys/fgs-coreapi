using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributes.Queries.ListAssetAttributes;

public sealed class ListAssetAttributesQueryHandler(IFgsAssetAttributeReadRepository readRepository)
    : IRequestHandler<ListAssetAttributesQuery, ApiResponse<PagedResult<FgsAssetAttributeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsAssetAttributeSummaryDto>>> Handle(
        ListAssetAttributesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsAssetAttributeSummaryDto>>.Ok(result);
    }
}
