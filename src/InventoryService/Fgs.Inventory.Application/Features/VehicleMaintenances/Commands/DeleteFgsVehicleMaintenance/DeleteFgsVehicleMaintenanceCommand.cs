using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VehicleMaintenances.Commands.DeleteFgsVehicleMaintenance;

public sealed record DeleteFgsVehicleMaintenanceCommand(long Id)
    : IRequest<ApiResponse<FgsVehicleMaintenanceDetailDto>>;
