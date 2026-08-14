using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Abstractions.InventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryItemDependencies.Queries.ListFgsInventoryItemDependencies;

public sealed class ListFgsInventoryItemDependenciesQueryHandler(
    IFgsInventoryItemDependencyReadRepository readRepository)
    : IRequestHandler<ListFgsInventoryItemDependenciesQuery, ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>> Handle(
        ListFgsInventoryItemDependenciesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListByInventoryItemIdAsync(request.InventoryItemId, cancellationToken);
        return ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>.Ok(result);
    }
}
