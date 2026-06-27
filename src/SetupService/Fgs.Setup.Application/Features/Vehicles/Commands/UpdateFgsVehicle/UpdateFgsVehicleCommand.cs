using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Commands.UpdateFgsVehicle;

public sealed record UpdateFgsVehicleCommand(long Id, FgsVehicleUpdateDto Dto)
    : IRequest<ApiResponse<FgsVehicleDetailDto>>;
