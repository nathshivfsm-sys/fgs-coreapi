using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryTransactions.Queries.ListInventoryTransactions;

public sealed record ListInventoryTransactionsQuery(
    InventoryListQuery Query,
    FgsInventoryTransactionListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsInventoryTransactionSummaryDto>>>;
