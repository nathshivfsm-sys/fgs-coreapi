using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Commands.CreateFgsVehicle;

public sealed record CreateFgsVehicleCommand(FgsVehicleCreateDto Dto)
    : IRequest<ApiResponse<FgsVehicleDetailDto>>;
