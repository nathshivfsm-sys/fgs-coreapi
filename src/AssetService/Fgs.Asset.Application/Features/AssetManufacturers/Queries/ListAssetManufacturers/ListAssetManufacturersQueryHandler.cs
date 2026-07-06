using Fgs.Asset.Application.Abstractions.AssetManufacturers;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetManufacturers.Queries.ListAssetManufacturers;

public sealed class ListAssetManufacturersQueryHandler(IFgsAssetManufacturerReadRepository readRepository)
    : IRequestHandler<ListAssetManufacturersQuery, ApiResponse<PagedResult<FgsAssetManufacturerSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsAssetManufacturerSummaryDto>>> Handle(
        ListAssetManufacturersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsAssetManufacturerSummaryDto>>.Ok(result);
    }
}
