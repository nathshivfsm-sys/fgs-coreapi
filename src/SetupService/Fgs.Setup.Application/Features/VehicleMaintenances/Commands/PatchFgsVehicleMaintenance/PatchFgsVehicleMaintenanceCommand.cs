using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Commands.PatchFgsVehicleMaintenance;

public sealed record PatchFgsVehicleMaintenanceCommand(long Id, FgsVehicleMaintenancePatchDto Dto)
    : IRequest<ApiResponse<FgsVehicleMaintenanceDetailDto>>;
