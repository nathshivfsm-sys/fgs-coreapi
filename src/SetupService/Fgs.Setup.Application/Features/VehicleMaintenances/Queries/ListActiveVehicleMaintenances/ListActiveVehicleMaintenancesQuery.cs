using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Queries.ListActiveVehicleMaintenances;

public sealed record ListActiveVehicleMaintenancesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsVehicleMaintenanceListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsVehicleMaintenanceSummaryDto>>>;
