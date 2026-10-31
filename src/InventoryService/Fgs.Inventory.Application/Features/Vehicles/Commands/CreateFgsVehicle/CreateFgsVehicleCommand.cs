using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vehicles.Commands.CreateFgsVehicle;

public sealed record CreateFgsVehicleCommand(FgsVehicleCreateDto Dto)
    : IRequest<ApiResponse<FgsVehicleDetailDto>>;
