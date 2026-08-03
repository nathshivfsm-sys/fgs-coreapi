using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Queries.ListVendorInventoryItems;

public sealed record ListVendorInventoryItemsQuery(
    InventoryListQuery Query,
    FgsVendorInventoryItemListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsVendorInventoryItemSummaryDto>>>;
