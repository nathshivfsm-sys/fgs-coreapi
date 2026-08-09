using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItems.Queries.ListInventoryItems;

public sealed class ListInventoryItemsQueryHandler(IFgsInventoryItemReadRepository readRepository)
    : IRequestHandler<ListInventoryItemsQuery, ApiResponse<PagedResult<FgsInventoryItemSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsInventoryItemSummaryDto>>> Handle(
        ListInventoryItemsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsInventoryItemSummaryDto>>.Ok(result);
    }
}
