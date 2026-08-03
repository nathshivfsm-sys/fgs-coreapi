using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryStocks.Queries.ListInventoryStocks;

public sealed record ListInventoryStocksQuery(
    InventoryListQuery Query,
    FgsInventoryStockListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsInventoryStockSummaryDto>>>;
