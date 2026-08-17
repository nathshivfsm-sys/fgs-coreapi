using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemDependencies.Queries.ListFgsInventoryItemDependencies;

public sealed record ListFgsInventoryItemDependenciesQuery(long InventoryItemId)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>>;
