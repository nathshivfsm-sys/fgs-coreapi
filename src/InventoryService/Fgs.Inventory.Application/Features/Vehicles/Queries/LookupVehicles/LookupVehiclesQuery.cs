using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vehicles.Queries.LookupVehicles;

public sealed record LookupVehiclesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsVehicleLookupDto>>>;
