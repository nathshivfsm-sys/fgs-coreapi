using Fgs.Inventory.Application.Features.InventoryItems.Dtos;

namespace Fgs.Inventory.Application.Abstractions.InventoryItemDependencies;

public interface IFgsInventoryItemDependencyReadRepository
{
    Task<FgsInventoryItemDependencyDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsInventoryItemDependencyDetailDto>> ListByInventoryItemIdAsync(
        long inventoryItemId,
        CancellationToken cancellationToken = default);
}
