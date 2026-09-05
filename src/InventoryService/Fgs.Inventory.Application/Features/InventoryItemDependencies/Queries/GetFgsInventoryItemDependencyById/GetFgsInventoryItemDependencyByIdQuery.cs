using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemDependencies.Queries.GetFgsInventoryItemDependencyById;

public sealed record GetFgsInventoryItemDependencyByIdQuery(long Id)
    : IRequest<ApiResponse<FgsInventoryItemDependencyDetailDto>>;
