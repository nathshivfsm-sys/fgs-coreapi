using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vehicles.Queries.ListActiveVehicles;

public sealed record ListActiveVehiclesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsVehicleListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsVehicleSummaryDto>>>;
