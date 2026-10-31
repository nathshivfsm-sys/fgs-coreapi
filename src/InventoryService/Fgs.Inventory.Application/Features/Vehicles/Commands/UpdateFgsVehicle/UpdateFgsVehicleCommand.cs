using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vehicles.Commands.UpdateFgsVehicle;

public sealed record UpdateFgsVehicleCommand(long Id, FgsVehicleUpdateDto Dto)
    : IRequest<ApiResponse<FgsVehicleDetailDto>>;
