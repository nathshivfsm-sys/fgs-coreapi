using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.PurchaseOrders;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.PurchaseOrders.Queries.ListPurchaseOrders;

public sealed class ListPurchaseOrdersQueryHandler(IFgsPurchaseOrderReadRepository readRepository)
    : IRequestHandler<ListPurchaseOrdersQuery, ApiResponse<PagedResult<FgsPurchaseOrderSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsPurchaseOrderSummaryDto>>> Handle(
        ListPurchaseOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsPurchaseOrderSummaryDto>>.Ok(result);
    }
}
