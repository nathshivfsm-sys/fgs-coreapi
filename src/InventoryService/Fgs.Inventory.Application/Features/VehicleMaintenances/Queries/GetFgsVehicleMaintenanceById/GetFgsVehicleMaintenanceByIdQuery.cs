using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VehicleMaintenances.Queries.GetFgsVehicleMaintenanceById;

public sealed record GetFgsVehicleMaintenanceByIdQuery(long Id)
    : IRequest<ApiResponse<FgsVehicleMaintenanceDetailDto>>;
