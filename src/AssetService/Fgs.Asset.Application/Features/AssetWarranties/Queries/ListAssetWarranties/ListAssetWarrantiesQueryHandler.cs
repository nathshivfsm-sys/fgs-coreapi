using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetWarranties.Queries.ListAssetWarranties;

public sealed class ListAssetWarrantiesQueryHandler(IFgsAssetWarrantyReadRepository readRepository)
    : IRequestHandler<ListAssetWarrantiesQuery, ApiResponse<PagedResult<FgsAssetWarrantySummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsAssetWarrantySummaryDto>>> Handle(
        ListAssetWarrantiesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsAssetWarrantySummaryDto>>.Ok(result);
    }
}
