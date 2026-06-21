using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Queries.ListVehicleMaintenances;

public sealed record ListVehicleMaintenancesQuery(
    SetupListQuery Query, FgsVehicleMaintenanceListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsVehicleMaintenanceSummaryDto>>>;
