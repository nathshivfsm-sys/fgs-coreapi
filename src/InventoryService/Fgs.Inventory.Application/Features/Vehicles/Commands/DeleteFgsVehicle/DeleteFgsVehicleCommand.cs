using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vehicles.Commands.DeleteFgsVehicle;

public sealed record DeleteFgsVehicleCommand(long Id)
    : IRequest<ApiResponse<FgsVehicleDetailDto>>;
