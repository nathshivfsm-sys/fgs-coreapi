using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Queries.GetFgsVehicleMaintenanceById;

public sealed record GetFgsVehicleMaintenanceByIdQuery(long Id)
    : IRequest<ApiResponse<FgsVehicleMaintenanceDetailDto>>;
