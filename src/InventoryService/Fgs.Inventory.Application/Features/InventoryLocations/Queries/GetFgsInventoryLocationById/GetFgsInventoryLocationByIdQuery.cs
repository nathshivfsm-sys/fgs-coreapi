using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Queries.GetFgsInventoryLocationById;

public sealed record GetFgsInventoryLocationByIdQuery(long Id)
    : IRequest<ApiResponse<FgsInventoryLocationDetailDto>>;
