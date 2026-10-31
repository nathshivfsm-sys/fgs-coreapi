using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vehicles.Queries.ListVehicles;

public sealed record ListVehiclesQuery(
    InventoryListQuery Query, FgsVehicleListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsVehicleSummaryDto>>>;
