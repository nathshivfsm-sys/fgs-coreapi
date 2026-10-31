using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VehicleMaintenances.Queries.LookupVehicleMaintenances;

public sealed record LookupVehicleMaintenancesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsVehicleMaintenanceLookupDto>>>;
