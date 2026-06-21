using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Commands.DeleteFgsVehicleMaintenance;

public sealed record DeleteFgsVehicleMaintenanceCommand(long Id)
    : IRequest<ApiResponse<FgsVehicleMaintenanceDetailDto>>;
