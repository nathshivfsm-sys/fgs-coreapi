using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VehicleMaintenances.Commands.UpdateFgsVehicleMaintenance;

public sealed record UpdateFgsVehicleMaintenanceCommand(long Id, FgsVehicleMaintenanceUpdateDto Dto)
    : IRequest<ApiResponse<FgsVehicleMaintenanceDetailDto>>;
