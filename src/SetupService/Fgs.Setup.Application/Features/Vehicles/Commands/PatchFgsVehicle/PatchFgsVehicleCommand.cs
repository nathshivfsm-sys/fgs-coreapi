using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Commands.PatchFgsVehicle;

public sealed record PatchFgsVehicleCommand(long Id, FgsVehiclePatchDto Dto)
    : IRequest<ApiResponse<FgsVehicleDetailDto>>;
