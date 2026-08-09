using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItems.Queries.ListInventoryItems;

public sealed record ListInventoryItemsQuery(
    InventoryListQuery Query,
    FgsInventoryItemListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsInventoryItemSummaryDto>>>;
