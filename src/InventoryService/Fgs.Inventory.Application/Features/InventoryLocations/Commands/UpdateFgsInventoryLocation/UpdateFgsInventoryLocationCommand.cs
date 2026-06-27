using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Commands.UpdateFgsInventoryLocation;

public sealed record UpdateFgsInventoryLocationCommand(long Id, FgsInventoryLocationUpdateDto Dto)
    : IRequest<ApiResponse<FgsInventoryLocationDetailDto>>;
