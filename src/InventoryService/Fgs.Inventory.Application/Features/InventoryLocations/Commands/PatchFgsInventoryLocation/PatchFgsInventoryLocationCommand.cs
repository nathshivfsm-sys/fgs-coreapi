using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Commands.PatchFgsInventoryLocation;

public sealed record PatchFgsInventoryLocationCommand(long Id, FgsInventoryLocationPatchDto Dto)
    : IRequest<ApiResponse<FgsInventoryLocationDetailDto>>;
