using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Queries.ListInventoryItemTypes;

public sealed record ListInventoryItemTypesQuery(
    InventoryListQuery Query,
    FgsInventoryItemTypeListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsInventoryItemTypeSummaryDto>>>;
