using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Queries.LookupVehicleMaintenances;

public sealed record LookupVehicleMaintenancesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsVehicleMaintenanceLookupDto>>>;
