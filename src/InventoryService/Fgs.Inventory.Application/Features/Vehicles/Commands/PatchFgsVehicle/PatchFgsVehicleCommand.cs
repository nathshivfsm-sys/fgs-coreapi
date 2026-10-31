using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vehicles.Commands.PatchFgsVehicle;

public sealed record PatchFgsVehicleCommand(long Id, FgsVehiclePatchDto Dto)
    : IRequest<ApiResponse<FgsVehicleDetailDto>>;
