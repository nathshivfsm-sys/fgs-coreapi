using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VehicleMaintenances.Commands.CreateFgsVehicleMaintenance;

public sealed record CreateFgsVehicleMaintenanceCommand(FgsVehicleMaintenanceCreateDto Dto)
    : IRequest<ApiResponse<FgsVehicleMaintenanceDetailDto>>;
