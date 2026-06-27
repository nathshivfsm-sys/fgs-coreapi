using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Queries.GetFgsVehicleById;

public sealed record GetFgsVehicleByIdQuery(long Id)
    : IRequest<ApiResponse<FgsVehicleDetailDto>>;
