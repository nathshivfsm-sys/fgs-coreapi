using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Queries.ListActiveInventoryLocations;

public sealed record ListActiveInventoryLocationsQuery(
    int Page = 1,
    int PageSize = 25,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Asc,
    string? Search = null,
    FgsInventoryLocationListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsInventoryLocationSummaryDto>>>;
