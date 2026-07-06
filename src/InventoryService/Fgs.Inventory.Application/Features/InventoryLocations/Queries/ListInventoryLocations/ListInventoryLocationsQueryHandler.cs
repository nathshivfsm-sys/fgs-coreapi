using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Queries.ListInventoryLocations;

public sealed class ListInventoryLocationsQueryHandler(IFgsInventoryLocationReadRepository readRepository)
    : IRequestHandler<ListInventoryLocationsQuery, ApiResponse<PagedResult<FgsInventoryLocationSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsInventoryLocationSummaryDto>>> Handle(
        ListInventoryLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsInventoryLocationSummaryDto>>.Ok(result);
    }
}
