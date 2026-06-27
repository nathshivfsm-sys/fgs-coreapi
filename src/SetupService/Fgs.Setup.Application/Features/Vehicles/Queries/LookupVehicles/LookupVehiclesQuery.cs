using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Queries.LookupVehicles;

public sealed record LookupVehiclesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsVehicleLookupDto>>>;
