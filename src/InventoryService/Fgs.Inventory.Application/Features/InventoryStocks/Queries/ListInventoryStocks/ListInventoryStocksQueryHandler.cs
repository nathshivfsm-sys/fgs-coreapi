using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryStocks;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryStocks.Queries.ListInventoryStocks;

public sealed class ListInventoryStocksQueryHandler(IFgsInventoryStockReadRepository readRepository)
    : IRequestHandler<ListInventoryStocksQuery, ApiResponse<PagedResult<FgsInventoryStockSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsInventoryStockSummaryDto>>> Handle(
        ListInventoryStocksQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsInventoryStockSummaryDto>>.Ok(result);
    }
}
