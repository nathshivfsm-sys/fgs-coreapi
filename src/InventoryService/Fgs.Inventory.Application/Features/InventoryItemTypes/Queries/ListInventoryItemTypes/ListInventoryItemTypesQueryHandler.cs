using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Queries.ListInventoryItemTypes;

public sealed class ListInventoryItemTypesQueryHandler(IFgsInventoryItemTypeReadRepository readRepository)
    : IRequestHandler<ListInventoryItemTypesQuery, ApiResponse<PagedResult<FgsInventoryItemTypeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsInventoryItemTypeSummaryDto>>> Handle(
        ListInventoryItemTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsInventoryItemTypeSummaryDto>>.Ok(result);
    }
}
