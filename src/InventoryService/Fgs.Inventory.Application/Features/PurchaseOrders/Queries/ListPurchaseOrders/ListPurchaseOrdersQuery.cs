using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.PurchaseOrders.Queries.ListPurchaseOrders;

public sealed record ListPurchaseOrdersQuery(
    InventoryListQuery Query,
    FgsPurchaseOrderListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsPurchaseOrderSummaryDto>>>;
