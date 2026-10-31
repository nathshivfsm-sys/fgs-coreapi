using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VehicleMaintenances.Queries.ListActiveVehicleMaintenances;

public sealed record ListActiveVehicleMaintenancesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsVehicleMaintenanceListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsVehicleMaintenanceSummaryDto>>>;
