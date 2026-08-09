using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItems.Commands.UpdateFgsInventoryItem;

public sealed record UpdateFgsInventoryItemCommand(long Id, FgsInventoryItemUpdateDto Dto)
    : IRequest<ApiResponse<FgsInventoryItemDetailDto>>;
