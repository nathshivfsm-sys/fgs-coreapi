using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Queries.ListVehicles;

public sealed record ListVehiclesQuery(
    SetupListQuery Query, FgsVehicleListFilters Filters)
    : IRequest<ApiResponse<PagedResult<FgsVehicleSummaryDto>>>;
