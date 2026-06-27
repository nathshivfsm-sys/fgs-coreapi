using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Commands.UpdateFgsVehicleMaintenance;

public sealed record UpdateFgsVehicleMaintenanceCommand(long Id, FgsVehicleMaintenanceUpdateDto Dto)
    : IRequest<ApiResponse<FgsVehicleMaintenanceDetailDto>>;
