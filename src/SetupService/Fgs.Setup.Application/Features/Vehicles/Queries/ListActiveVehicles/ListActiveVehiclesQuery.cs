using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Queries.ListActiveVehicles;

public sealed record ListActiveVehiclesQuery(
    int Page = 1, int PageSize = 25, string? SortBy = null, SortDirection SortDirection = SortDirection.Asc, string? Search = null, FgsVehicleListFilters? Filters = null)
    : IRequest<ApiResponse<PagedResult<FgsVehicleSummaryDto>>>;
