using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemTypes.Queries.GetFgsInventoryItemTypeById;

public sealed record GetFgsInventoryItemTypeByIdQuery(long Id)
    : IRequest<ApiResponse<FgsInventoryItemTypeDetailDto>>;
