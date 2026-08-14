using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.UpdateFgsInventoryItemDependencies;

public sealed record UpdateFgsInventoryItemDependenciesCommand(FgsInventoryItemDependencyReplaceDto Dto)
    : IRequest<ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>>;
