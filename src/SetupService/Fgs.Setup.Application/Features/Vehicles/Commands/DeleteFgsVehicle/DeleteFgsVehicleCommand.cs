using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Commands.DeleteFgsVehicle;

public sealed record DeleteFgsVehicleCommand(long Id)
    : IRequest<ApiResponse<FgsVehicleDetailDto>>;
