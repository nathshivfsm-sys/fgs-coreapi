using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Queries.ListTruckStockTemplateItems;

public sealed record ListTruckStockTemplateItemsQuery(
    long TemplateId,
    InventoryListQuery Query,
    FgsTruckStockTemplateItemListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsTruckStockTemplateItemSummaryDto>>>;
