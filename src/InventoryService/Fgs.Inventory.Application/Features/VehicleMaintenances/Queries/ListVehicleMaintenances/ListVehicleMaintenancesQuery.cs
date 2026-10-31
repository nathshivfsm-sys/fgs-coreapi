using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VehicleMaintenances.Queries.ListVehicleMaintenances;

public sealed record ListVehicleMaintenancesQuery(
    InventoryListQuery Query, FgsVehicleMaintenanceListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsVehicleMaintenanceSummaryDto>>>;
