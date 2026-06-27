using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Commands.CreateFgsVehicleMaintenance;

public sealed record CreateFgsVehicleMaintenanceCommand(FgsVehicleMaintenanceCreateDto Dto)
    : IRequest<ApiResponse<FgsVehicleMaintenanceDetailDto>>;
