using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Commands.CreateFgsInventoryLocation;

public sealed record CreateFgsInventoryLocationCommand(FgsInventoryLocationCreateDto Dto)
    : IRequest<ApiResponse<FgsInventoryLocationDetailDto>>;
