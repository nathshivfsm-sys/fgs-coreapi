using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.CreateFgsInventoryItemType;

public sealed record CreateFgsInventoryItemTypeCommand(FgsInventoryItemTypeCreateDto Dto)
    : IRequest<ApiResponse<FgsInventoryItemTypeDetailDto>>;
