using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryTransactions;
using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryTransactions.Queries.ListInventoryTransactions;

public sealed class ListInventoryTransactionsQueryHandler(IFgsInventoryTransactionReadRepository readRepository)
    : IRequestHandler<ListInventoryTransactionsQuery, ApiResponse<PagedResult<FgsInventoryTransactionSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsInventoryTransactionSummaryDto>>> Handle(
        ListInventoryTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<FgsInventoryTransactionSummaryDto>>.Ok(result);
    }
}
