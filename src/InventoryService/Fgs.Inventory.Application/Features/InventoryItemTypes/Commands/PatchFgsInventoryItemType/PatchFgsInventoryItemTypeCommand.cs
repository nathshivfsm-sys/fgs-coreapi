using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.PatchFgsInventoryItemType;

public sealed record PatchFgsInventoryItemTypeCommand(long Id, FgsInventoryItemTypePatchDto Dto)
    : IRequest<ApiResponse<FgsInventoryItemTypeDetailDto>>;
