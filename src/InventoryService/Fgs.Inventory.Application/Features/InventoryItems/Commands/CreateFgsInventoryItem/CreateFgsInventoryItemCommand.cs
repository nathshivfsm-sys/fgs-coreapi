using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItems.Commands.CreateFgsInventoryItem;

public sealed record CreateFgsInventoryItemCommand(FgsInventoryItemCreateDto Dto)
    : IRequest<ApiResponse<FgsInventoryItemDetailDto>>;
