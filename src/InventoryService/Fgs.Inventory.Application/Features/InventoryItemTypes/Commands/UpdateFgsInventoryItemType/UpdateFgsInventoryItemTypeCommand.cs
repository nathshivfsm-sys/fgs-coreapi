using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.UpdateFgsInventoryItemType;

public sealed record UpdateFgsInventoryItemTypeCommand(long Id, FgsInventoryItemTypeUpdateDto Dto)
    : IRequest<ApiResponse<FgsInventoryItemTypeDetailDto>>;
