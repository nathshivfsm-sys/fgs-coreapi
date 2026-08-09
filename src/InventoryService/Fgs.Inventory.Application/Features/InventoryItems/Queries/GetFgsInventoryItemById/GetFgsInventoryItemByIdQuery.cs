using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItems.Queries.GetFgsInventoryItemById;

public sealed record GetFgsInventoryItemByIdQuery(long Id)
    : IRequest<ApiResponse<FgsInventoryItemDetailDto>>;
