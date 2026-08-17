using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.CreateFgsInventoryItemDependencies;

public sealed record CreateFgsInventoryItemDependenciesCommand(FgsInventoryItemDependencyReplaceDto Dto)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>>;
