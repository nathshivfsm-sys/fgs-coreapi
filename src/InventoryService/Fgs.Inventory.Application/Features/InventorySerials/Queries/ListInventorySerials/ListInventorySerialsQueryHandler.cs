using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventorySerials;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySerials.Queries.ListInventorySerials;

public sealed class ListInventorySerialsQueryHandler(IFgsInventorySerialReadRepository readRepository)
    : IRequestHandler<ListInventorySerialsQuery, ApiResponse<PagedResult<FgsInventorySerialSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsInventorySerialSummaryDto>>> Handle(
        ListInventorySerialsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsInventorySerialSummaryDto>>.Ok(result);
    }
}
