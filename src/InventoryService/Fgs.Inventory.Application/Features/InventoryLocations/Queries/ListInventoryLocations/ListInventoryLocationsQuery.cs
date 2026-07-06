using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Queries.ListInventoryLocations;

public sealed record ListInventoryLocationsQuery(
    InventoryListQuery Query,
    FgsInventoryLocationListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsInventoryLocationSummaryDto>>>;
