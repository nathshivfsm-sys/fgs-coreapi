using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vehicles.Queries.GetFgsVehicleById;

public sealed record GetFgsVehicleByIdQuery(long Id)
    : IRequest<ApiResponse<FgsVehicleDetailDto>>;
